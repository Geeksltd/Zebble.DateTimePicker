namespace Zebble
{
    using System;
    using System.Threading.Tasks;
    using Olive;

    public partial class TimePicker : Picker
    {
        const int AM = 1, PM = 2;

        public static string DefaultFormat = "hh:mm tt";
        public int MinuteInterval = 5;
        public TimeFormat TimeFormat = TimeFormat.AMPM;

        TimeSpan? selectedValue;
        public readonly AsyncEvent SelectedValueChanged = new AsyncEvent();

        public TimePicker() => SelectedValueChanged.Handle(() => RaiseInputChanged(nameof(SelectedValue)));

        public string TextFormat { get; set; } = DefaultFormat;

        public TimeSpan? SelectedValue
        {
            get => selectedValue;
            set
            {
                selectedValue = value;
                SetSelectedText(SelectedValue.Get(v => LocalTime.Today.Add(v.Value).ToString(TextFormat)));
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

            if (TimeFormat == TimeFormat.AMPM && dialog.AmPmRotator.SelectedItem.Value == AM && hour == 12)
                hour = 0;

            SelectedValue = new TimeSpan(hour, minute, 0);
            await SelectedValueChanged.Raise();

            await hide;
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
