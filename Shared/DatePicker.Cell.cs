namespace Zebble
{
    using System;
    using System.Threading.Tasks;

    partial class DatePicker
    {
        public class Cell : View, IListViewItem<Item>
        {
            public readonly TextView Label = new TextView { Id = "Label" };
            public Item Item { get; set; }

            public override string ToString() => "DatePicker-Cell - " + Item;

            public override async Task OnInitializing()
            {
                await base.OnInitializing();
                await Add(Label.Set(x => x.Text = Item.Text));
            }
        }
    }
}