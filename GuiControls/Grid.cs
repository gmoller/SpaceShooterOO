using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    public class GridColumns : IEnumerable<Label>
    {
        private readonly List<Label> _items;

        public int Count => _items.Count;

        public Label this[int index] => _items[index];

        public GridColumns(params Label[] items)
        {
            _items = new List<Label>();
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }

        public void Add(Label item)
        {
            _items.Add(item);
        }

        public IEnumerator<Label> GetEnumerator()
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

    public class GridRows : IEnumerable<GridRow>
    {
        private readonly List<GridRow> _items;

        public int Count => _items.Count;

        public GridRow this[int index] => _items[index];

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

    public class Grid
    {
        public SpriteFont RowsFont { get; }
        public Vector2 Position { get; }

        public GridColumns Columns { get; }
        public GridRows Rows { get; }

        public Grid(SpriteFont headerFont, SpriteFont rowsFont, Color columnHeaderColor, Vector2 position, bool textShadow, ColumnHeaders columnHeaders)
        {
            RowsFont = rowsFont;
            Position = position;

            Columns = new GridColumns();
            foreach (ColumnHeader columnHeader in columnHeaders)
            {
                var label = new Label(headerFont, VerticalAlignment.Top, columnHeader.HorizontalAlignment, new Vector2(columnHeader.X, position.Y), columnHeader.Name, columnHeaderColor) { TextShadow = textShadow };
                Columns.Add(label);
            }

            Rows = new GridRows();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw column headers
            foreach (Label columnHeader in Columns)
            {
                columnHeader.Draw(spriteBatch);
            }

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

    public class ColumnHeader
    {
        public string Name { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public float X { get; set; }
    }

    public class ColumnHeaders : IEnumerable<ColumnHeader>
    {
        private readonly List<ColumnHeader> _items;

        public int Count => _items.Count;

        public ColumnHeader this[int index] => _items[index];

        public ColumnHeaders(params ColumnHeader[] items)
        {
            _items = new List<ColumnHeader>();
            foreach (ColumnHeader item in items)
            {
                _items.Add(item);
            }
        }

        public IEnumerator<ColumnHeader> GetEnumerator()
        {
            foreach (ColumnHeader item in _items)
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

    public class GridRow
    {
        private readonly List<Label> _items;

        public GridRow(Grid parent, Color color, params GridColumn[] gridColumns)
        {
            _items = new List<Label>();
            float y = (parent.Position.Y + 25.0f) + (parent.Rows.Count * 15.0f);
            foreach (GridColumn gridColumn in gridColumns)
            {
                float x = gridColumn.X;
                var label = new Label(parent.RowsFont, VerticalAlignment.Top, gridColumn.HorizontalAlignment, new Vector2(x, y), gridColumn.Text, color);
                _items.Add(label);
            }
        }

        // list of labels
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _items)
            {
                item.Draw(spriteBatch);
            }
        }
    }
}