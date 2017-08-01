namespace Zebble
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    partial class DatePicker
    {
        public class Dialog : Zebble.Dialog
        {
            DatePicker Picker;
            public readonly AsyncEvent Accepted = new AsyncEvent();

            public readonly Stack RotatorsRow = new Stack(RepeatDirection.Horizontal).Id("RotatorsRow");

            public readonly Rotator<Item, Cell> DaysRotator = new Rotator<Item, Cell>().Set(x => x.Css.Width(40.Percent()));
            public readonly Rotator<Item, Cell> MonthsRotator = new Rotator<Item, Cell>().Set(x => x.Css.Width(30.Percent()));
            public readonly Rotator<Item, Cell> YearsRotator = new Rotator<Item, Cell>().Set(x => x.Css.Width(30.Percent()));

            public readonly Button CancelButton = new Button { Text = "Cancel" };
            public readonly Button RemoveButton = new Button { Text = "Remove" };
            public readonly Button OkButton = new Button { Text = "OK", CssClass = "primary-button" };

            public Dialog(DatePicker picker)
            {
                Picker = picker;
                ScrollContent = false;
                Title.Text = "Today: " + LocalTime.Now.ToString("dd MMM yyyy");
            }

            public override async Task OnInitializing()
            {
                await base.OnInitializing();

                await DaysRotator.SetSource(GetDays());
                await MonthsRotator.SetSource(GetMonths());
                await YearsRotator.SetSource(GetYears());

                MonthsRotator.SelectionChanged.Handle(() => UIWorkBatch.Run(UpdateDaysBasedOnMonthAndYear));
                YearsRotator.SelectionChanged.Handle(() => UIWorkBatch.Run(UpdateDaysBasedOnMonthAndYear));

                await Content.Add(RotatorsRow);
                await RotatorsRow.AddRange(new[] { DaysRotator, MonthsRotator, YearsRotator });

                if (Picker.AllowNull && Picker.SelectedValue.HasValue)
                    await ButtonsRow.Add(RemoveButton.On(x => x.Tapped, RemoveButtonTapped));
                else
                    await ButtonsRow.Add(CancelButton.On(x => x.Tapped, () => Nav.HidePopUp()));

                await ButtonsRow.Add(OkButton.On(x => x.Tapped, Accepted.Raise));

                await WhenShown(LoadCurrentValue);
            }

            IEnumerable<Item> GetYears()
            {
                foreach (var y in Enumerable.Range(Picker.YearFrom, Math.Max(Picker.YearTo - Picker.YearFrom, 1)))
                    yield return new Item { Value = y, Text = y.ToString("0000") };
            }

            IEnumerable<Item> GetMonths()
            {
                foreach (var m in Enumerable.Range(1, 12))
                    yield return new Item { Value = m, Text = new DateTime(1, m, 1).ToString(Picker.MonthFormat) };
            }

            IEnumerable<Item> GetDays()
            {
                var date = Picker.SelectedValue ?? LocalTime.Now;

                if ((YearsRotator.SelectedItem?.Value ?? -1) > 0)
                    if ((MonthsRotator.SelectedItem?.Value ?? -1) > 0)
                        date = new DateTime(YearsRotator.SelectedItem.Value, MonthsRotator.SelectedItem.Value, 1);

                foreach (var d in Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)))
                    yield return new Item { Value = d, Text = new DateTime(date.Year, date.Month, d).ToString(Picker.DayFormat) };
            }

            Task RemoveButtonTapped()
            {
                Picker.SelectedValue = null;
                return Nav.HidePopUp();
            }

            async Task UpdateDaysBasedOnMonthAndYear()
            {
                var daysSource = GetDays().ToArray();

                var selectedDay = DaysRotator.SelectedItem?.Value ?? -1;
                selectedDay = selectedDay.LimitMax(daysSource.Max(item => item.Value));

                var itemCounter = 0;
                foreach (var item in DaysRotator.List.ItemViews.Where(item => item.Item.Text.HasValue()).ToArray())
                {
                    if (itemCounter < daysSource.Length)
                    {
                        var sourceItem = daysSource[itemCounter++];

                        item.Item.Text = sourceItem.Text;
                        item.Item.Value = sourceItem.Value;

                        item.Label.Text = sourceItem.Text;
                    }
                    else await DaysRotator.List.Remove(item.Item);
                }

                var rotatorCount = DaysRotator.List.ItemViews.Count(item => item.Item.Text.HasValue());
                if (daysSource.Length > rotatorCount)
                    foreach (var item in daysSource.Skip(rotatorCount))
                        await DaysRotator.Append(item);

                DaysRotator.PreSelect(item => item.Value == selectedDay);
            }

            protected void LoadCurrentValue()
            {
                var date = Picker.SelectedValue ?? LocalTime.Today;

                YearsRotator.PreSelect(x => x.Value == date.Year);
                MonthsRotator.PreSelect(x => x.Value == date.Month);
                DaysRotator.PreSelect(x => x.Value == date.Day);
            }

            public override void Dispose()
            {
                Accepted?.Dispose();
                base.Dispose();
            }
        }
    }
}