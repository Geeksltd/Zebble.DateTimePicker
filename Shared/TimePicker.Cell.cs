namespace Zebble
{
    using System;
    using System.Threading.Tasks;

    partial class TimePicker
    {
        public class Cell : View, IListViewItem<Item>
        {
            public readonly TextView Label = new TextView { Id = "Label" };
            public Item Item { get; set; }

            public int Value => Item.Value;

            public override async Task OnInitializing()
            {
                await base.OnInitializing();
                await Add(Label.Set(x => x.Text = Item.Text));
            }
        }
    }
}