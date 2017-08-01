namespace Zebble
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    partial class TimePicker
    {
        public class Dialog : Zebble.Dialog
        {
            TimePicker Picker;
            public readonly AsyncEvent Accepted = new AsyncEvent();
            public readonly Stack RotatorsRow = new Stack(RepeatDirection.Horizontal).Id("RotatorsRow");

            public readonly Rotator<Item, Cell> HoursRotator = new Rotator<Item, Cell>().Id("HoursRotator");
            public readonly Rotator<Item, Cell> MinutesRotator = new Rotator<Item, Cell>().Id("MinutesRotator");
            public readonly Rotator<Item, Cell> AmPmRotator = new Rotator<Item, Cell>().Id("AmPmRotator");

            public readonly TextView ClockSpliter = new TextView { Text = ":" }
            .TextAlignment(Alignment.Middle).Size(10.Percent(), 100.Percent());
            public readonly Button CancelButton = new Button { Text = "Cancel" };
            public readonly Button RemoveButton = new Button { Text = "Remove" };
            public readonly Button OkButton = new Button { Text = "OK", CssClass = "primary-button" };

            public Dialog(TimePicker picker)
            {
                Picker = picker;
                ScrollContent = false;
                Title.Text = "Pick a time";
            }

            public override async Task OnInitializing()
            {
                await base.OnInitializing();

                if (Picker.SelectedValue == null)
                    Picker.SelectedValue = LocalTime.Now.Hour.Hours().Add(LocalTime.Now.Minute.RoundUpToNearest(5).Minutes());

                //Hours
                var hoursRange = Picker.TimeFormat == TimeFormat.Twentyfour ? Enumerable.Range(1, 23) : Enumerable.Range(1, 12);
                var hours = hoursRange.Select(hour => new Item { Value = hour, Text = hour.ToString("00") });
                await HoursRotator.SetSource(hours);

                //Minutes
                var minutes = Enumerable.Range(0, 60).Where(minute => (minute % Picker.MinuteInterval) == 0)
                    .Select(minute => new Item { Value = minute, Text = minute.ToString("00") });
                await MinutesRotator.SetSource(minutes);
                MinutesRotator.Style.Ignored = Picker.TimeFormat != TimeFormat.AMPM;

                //TimeMode
                if (Picker.TimeFormat == TimeFormat.AMPM)
                    await AmPmRotator.SetSource(new[] { new Item { Value = AM, Text = "AM" }, new Item { Value = PM, Text = "PM" } });

                await Content.Add(RotatorsRow);

                await RotatorsRow.AddRange(new View[] { HoursRotator, ClockSpliter, MinutesRotator, AmPmRotator });

                if (Picker.AllowNull && Picker.SelectedValue.HasValue)
                    await ButtonsRow.Add(RemoveButton.On(x => x.Tapped, RemoveButtonTapped));
                else
                    await ButtonsRow.Add(CancelButton.On(x => x.Tapped, () => Nav.HidePopUp()));

                await ButtonsRow.Add(OkButton.On(x => x.Tapped, Accepted.Raise));

                await WhenShown(LoadCurrentValue);
            }

            Task RemoveButtonTapped()
            {
                Picker.SelectedValue = null;
                return Nav.HidePopUp();
            }

            protected void LoadCurrentValue()
            {
                var time = Picker.SelectedValue.Value;

                var hours = time.Hours;
                if (Picker.TimeFormat == TimeFormat.AMPM && hours > 12) hours -= 12;

                HoursRotator.PreSelect(x => x.Value == hours);
                MinutesRotator.PreSelect(x => x.Value == time.Minutes);

                if (Picker.TimeFormat == TimeFormat.AMPM)
                    AmPmRotator.PreSelect(x => x.Value == (time.TotalHours >= 12 ? PM : AM));
            }

            public override void Dispose()
            {
                Accepted?.Dispose();
                base.Dispose();
            }
        }

    }
}