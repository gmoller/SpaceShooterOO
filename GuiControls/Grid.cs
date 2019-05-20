using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    public class Grid
    {
        public SpriteFont RowsFont { get; }
        public Vector2 Position { get; }

        public GridColumns ColumnsDefinitions { get; }
        public GridColumnRow Columns { get; }
        public GridRows Rows { get; }

        public Grid(SpriteFont headerFont, SpriteFont rowsFont, Color columnHeaderColor, Vector2 position, bool textShadow, GridColumns gridColumns)
        {
            RowsFont = rowsFont;
            Position = position;

            ColumnsDefinitions = gridColumns;
            Columns = new GridColumnRow(this, headerFont, columnHeaderColor, textShadow, gridColumns);
            Rows = new GridRows();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw column headers
            Columns.Draw(spriteBatch);

            // draw rows
            foreach (GridRow row in Rows)
            {
                row.Draw(spriteBatch);
            }
        }

        public void ClearRows()
        {
            Rows.Clear();
        }

        public void AddRow(GridRow row)
        {
            Rows.Add(row);
        }
    }

    public class GridColumns : IEnumerable<GridColumn>
    {
        private readonly List<GridColumn> _items;

        public int Count => _items.Count;

        public GridColumn this[int index] => _items[index];

        public GridColumns()
        {
            _items = new List<GridColumn>();
        }

        public GridColumns(params GridColumn[] items)
        {
            _items = new List<GridColumn>();
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }

        public void Add(GridColumn item)
        {
            _items.Add(item);
        }

        public IEnumerator<GridColumn> GetEnumerator()
        {
            foreach (var item in _items)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class GridColumn
    {
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public string Text { get; set; }
        public float X { get; set; }
    }

    public class GridRows : IEnumerable<GridRow>
    {
        private readonly List<GridRow> _items;

        public int Count => _items.Count;

        public GridRow this[int index] => _items[index];

        public GridRows()
        {
            _items = new List<GridRow>();
        }

        public GridRows(params GridRow[] items)
        {
            _items = new List<GridRow>();
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void Add(GridRow item)
        {
            _items.Add(item);
        }

        public IEnumerator<GridRow> GetEnumerator()
        {
            foreach (var item in _items)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class GridRow
    {
        protected List<Label> Items;

        public GridRow()
        {
        }

        public GridRow(Grid parent, SpriteFont font, Color color, GridColumns gridColumns)
        {
            Items = new List<Label>();
            float y = (parent.Position.Y + 25.0f) + (parent.Rows.Count * 15.0f);
            foreach (GridColumn gridColumn in gridColumns)
            {
                float x = gridColumn.X;
                var label = new Label(font, VerticalAlignment.Top, gridColumn.HorizontalAlignment, new Vector2(x, y), gridColumn.Text, color);
                Items.Add(label);
            }
        }

        public GridRow(Grid parent, SpriteFont font, Color color, params string[] texts)
        {
            Items = new List<Label>();
            float y = (parent.Position.Y + 25.0f) + (parent.Rows.Count * 15.0f);

            int i = 0;
            foreach (string text in texts)
            {
                GridColumn gridColumn = parent.ColumnsDefinitions[i++];
                float x = gridColumn.X;
                var label = new Label(font, VerticalAlignment.Top, gridColumn.HorizontalAlignment, new Vector2(x, y), text, color);
                Items.Add(label);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in Items)
            {
                item.Draw(spriteBatch);
            }
        }
    }

    public class GridColumnRow : GridRow
    {
        public GridColumnRow(Grid parent, SpriteFont font, Color color, bool textShadow, GridColumns gridColumns)
        {
            Items = new List<Label>();
            float y = parent.Position.Y;
            foreach (GridColumn gridColumn in gridColumns)
            {
                float x = gridColumn.X;
                var label = new Label(font, VerticalAlignment.Top, gridColumn.HorizontalAlignment, new Vector2(x, y), gridColumn.Text, color) { TextShadow = textShadow };
                Items.Add(label);
            }
        }
    }
}