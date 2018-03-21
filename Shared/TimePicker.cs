namespace Zebble
{
    using System;
    using System.Threading.Tasks;

    public partial class TimePicker : Picker, FormField.IPlaceHolderControl, FormField.IControl
    {
        const int AM = 1;
        const int PM = 2;

        public static string DefaultFormat = "hh:mm tt";
        public int MinuteInterval = 5;
        public TimeFormat TimeFormat = TimeFormat.AMPM;

        TimeSpan? selectedValue;
        public readonly AsyncEvent SelectedValueChanged = new AsyncEvent();

        public string TextFormat { get; set; } = DefaultFormat;

        public TimeSpan? SelectedValue
        {
            get => selectedValue;
            set
            {
                selectedValue = value;
                SelectedText = value.Get(v => LocalTime.Today.Add(v.Value).ToString(TextFormat));
            }
        }

        protected override Zebble.Dialog CreateDialog()
        {
            var result = new Dialog(this);
            result.Accepted.Handle(() => OnSelectionChanged(result));
            return result;
        }

        async Task OnSelectionChanged(Dialog dialog)
        {
            var hide = Nav.HidePopUp();

            var minute = dialog.MinutesRotator.SelectedItem.Value;
            var hour = dialog.HoursRotator.SelectedItem.Value;

            if (TimeFormat == TimeFormat.AMPM && dialog.AmPmRotator.SelectedItem.Value == PM && hour < 12)
                hour += 12;

            SelectedValue = new TimeSpan(hour, minute, 0);
            await SelectedValueChanged.Raise();

            await hide;
        }

        object FormField.IControl.Value
        {
            get => SelectedValue;
            set => SelectedValue = (TimeSpan?)value;
        }

        public override void Dispose()
        {
            SelectedValueChanged?.Dispose();
            base.Dispose();
        }

        public class Item
        {
            public int Value { get; set; } = -1;
            public string Text { get; set; }
            public override string ToString() => Text;
        }
    }
}
