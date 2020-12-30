namespace Zebble
{
    using System;
    using System.Threading.Tasks;
    using Olive;

    public partial class DatePicker : Picker, FormField.IPlaceHolderControl, FormField.IControl, IBindableInput
    {
        // Formatting
        public string TextFormat = "dd MMM yyyy";
        public string DayFormat = "ddd, dd";
        public string MonthFormat = "MMM";

        public int YearFrom = LocalTime.Today.Year;
        public int YearTo = LocalTime.Today.Year + 10;

        DateTime? selectedValue;
        public readonly AsyncEvent SelectedValueChanged = new AsyncEvent();

        public DateTime? SelectedValue
        {
            get => selectedValue;
            set
            {
                selectedValue = value; 
                SelectedText = selectedValue.ToString(TextFormat);
            }
        }

        object FormField.IControl.Value
        {
            get => SelectedValue;
            set => SelectedValue = (DateTime?)value;
        }

        public void AddBinding(Bindable bindable) => SelectedValueChanged.Handle(() => bindable.SetUserValue(SelectedValue));

        protected override Zebble.Dialog CreateDialog()
        {
            var result = new Dialog(this) { Id = "Dialog" };
            result.Accepted.Handle(() => OnSelectionChanged(result));
            return result;
        }

        async Task OnSelectionChanged(Dialog dialog)
        {
            var hide = Nav.HidePopUp();

            var year = dialog.YearsRotator.SelectedItem.Value;
            var month = dialog.MonthsRotator.SelectedItem.Value;
            var day = dialog.DaysRotator.SelectedItem.Value;

            SelectedValue = new DateTime(year, month, day);
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