using System.Drawing.Drawing2D;

namespace Gestion_Universite;

public static class UITheme
{
    public static readonly Color PrimaryRed = Color.FromArgb(139, 0, 0); // #8B0000
    public static readonly Color HeaderRed = Color.FromArgb(168, 12, 12);  // #A80C0C
    public static readonly Color SidebarRed = Color.FromArgb(120, 0, 0);  // #780000
    public static readonly Color SidebarHover = Color.FromArgb(150, 0, 0);
    public static readonly Color AppBg = Color.FromArgb(246, 247, 249);
    public static readonly Color CardBg = Color.White;
    public static readonly Color BorderColor = Color.FromArgb(225, 228, 232);
    public static readonly Color TextPrimary = Color.FromArgb(33, 37, 41);
    public static readonly Color TextMuted = Color.FromArgb(108, 117, 125);

    // Badges
    public static readonly Color BadgeGreenBg = Color.FromArgb(212, 237, 218);
    public static readonly Color BadgeGreenText = Color.FromArgb(21, 87, 36);

    public static readonly Color BadgeGrayBg = Color.FromArgb(226, 227, 229);
    public static readonly Color BadgeGrayText = Color.FromArgb(56, 61, 65);

    public static readonly Color BadgeAmberBg = Color.FromArgb(255, 243, 205);
    public static readonly Color BadgeAmberText = Color.FromArgb(133, 100, 4);

    public static Font FontRegular(float size = 9f) => new("Segoe UI", size, FontStyle.Regular);
    public static Font FontBold(float size = 9f) => new("Segoe UI", size, FontStyle.Bold);
    public static Font FontSemiBold(float size = 9.5f) => new("Segoe UI Semibold", size, FontStyle.Regular);

    public static Panel CreateTitleBar(Form form, string title, string iconSymbol = "🎓", bool isDialog = false)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 42,
            BackColor = HeaderRed,
            Padding = new Padding(12, 0, 10, 0)
        };

        var lblIcon = new Label
        {
            Text = iconSymbol,
            Font = new Font("Segoe UI Emoji", 14f),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(10, 8)
        };

        var lblTitle = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(42, 10)
        };

        // Window controls
        var btnClose = new Button
        {
            Text = "✕",
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(34, 28),
            Cursor = Cursors.Hand,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(panel.Width - 44, 7)
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 30, 30);
        btnClose.Click += (s, e) => form.Close();

        panel.Controls.Add(lblIcon);
        panel.Controls.Add(lblTitle);
        panel.Controls.Add(btnClose);

        if (!isDialog)
        {
            var btnMax = new Button
            {
                Text = "□",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(34, 28),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panel.Width - 80, 7)
            };
            btnMax.FlatAppearance.BorderSize = 0;
            btnMax.FlatAppearance.MouseOverBackColor = SidebarHover;
            btnMax.Click += (s, e) => form.WindowState = form.WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;

            var btnMin = new Button
            {
                Text = "–",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(34, 28),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panel.Width - 116, 7)
            };
            btnMin.FlatAppearance.BorderSize = 0;
            btnMin.FlatAppearance.MouseOverBackColor = SidebarHover;
            btnMin.Click += (s, e) => form.WindowState = FormWindowState.Minimized;

            panel.Controls.Add(btnMax);
            panel.Controls.Add(btnMin);

            // Responsive positioning on resize
            panel.Resize += (s, e) =>
            {
                btnClose.Location = new Point(panel.Width - 44, 7);
                btnMax.Location = new Point(panel.Width - 80, 7);
                btnMin.Location = new Point(panel.Width - 116, 7);
            };
        }
        else
        {
            panel.Resize += (s, e) =>
            {
                btnClose.Location = new Point(panel.Width - 44, 7);
            };
        }

        // Enable dragging form with title bar
        bool dragging = false;
        Point dragCursorPoint = Point.Empty;
        Point dragFormPoint = Point.Empty;

        panel.MouseDown += (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = form.Location;
            }
        };

        panel.MouseMove += (s, e) =>
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                form.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        };

        panel.MouseUp += (s, e) => dragging = false;

        return panel;
    }

    public static Panel CreateCard(string? title = null, string? icon = null)
    {
        var card = new Panel
        {
            BackColor = CardBg,
            Padding = new Padding(16),
            Margin = new Padding(8)
        };

        card.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(BorderColor, 1);
            var rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
            e.Graphics.DrawRectangle(pen, rect);
        };

        if (!string.IsNullOrEmpty(title))
        {
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 35,
                BackColor = Color.Transparent
            };

            var lbl = new Label
            {
                Text = (icon != null ? $"{icon}  " : "") + title,
                Font = FontBold(11f),
                ForeColor = PrimaryRed,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            headerPanel.Controls.Add(lbl);
            card.Controls.Add(headerPanel);
        }

        return card;
    }

    public static Button CreatePrimaryButton(string text, string? icon = null)
    {
        var btn = new Button
        {
            Text = (icon != null ? $"{icon} " : "") + text,
            BackColor = PrimaryRed,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = FontBold(9.5f),
            Cursor = Cursors.Hand,
            Height = 36,
            Padding = new Padding(12, 0, 12, 0),
            AutoSize = true
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(170, 0, 0);
        return btn;
    }

    public static Button CreateSecondaryButton(string text, string? icon = null)
    {
        var btn = new Button
        {
            Text = (icon != null ? $"{icon} " : "") + text,
            BackColor = Color.White,
            ForeColor = TextPrimary,
            FlatStyle = FlatStyle.Flat,
            Font = FontBold(9f),
            Cursor = Cursors.Hand,
            Height = 36,
            Padding = new Padding(10, 0, 10, 0),
            AutoSize = true
        };
        btn.FlatAppearance.BorderColor = BorderColor;
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        return btn;
    }

    public static void StyleDataGridView(DataGridView dgv)
    {
        dgv.BackgroundColor = Color.White;
        dgv.BorderStyle = BorderStyle.None;
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgv.GridColor = Color.FromArgb(240, 242, 245);
        dgv.RowHeadersVisible = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.MultiSelect = false;
        dgv.AllowUserToAddRows = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.AllowUserToResizeRows = false;
        dgv.RowTemplate.Height = 40;
        dgv.EnableHeadersVisualStyles = false;
        dgv.Font = FontRegular(9.5f);

        // Header style
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgv.ColumnHeadersHeight = 38;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 248, 250);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
        dgv.ColumnHeadersDefaultCellStyle.Font = FontBold(9.5f);
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

        // Rows style
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 237, 237);
        dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
        dgv.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 253, 254);

        // Custom status badge painting
        dgv.CellPainting += (s, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgv.Columns[e.ColumnIndex].Name == "ColStatut")
            {
                e.PaintBackground(e.CellBounds, true);
                var val = e.Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(val))
                {
                    Color bg, fg;
                    if (val.Equals("Active", StringComparison.OrdinalIgnoreCase) ||
                        val.Equals("Actif", StringComparison.OrdinalIgnoreCase) ||
                        val.Equals("Ouverte", StringComparison.OrdinalIgnoreCase) ||
                        val.Equals("Disponible", StringComparison.OrdinalIgnoreCase))
                    {
                        bg = BadgeGreenBg;
                        fg = BadgeGreenText;
                    }
                    else if (val.Equals("En maintenance", StringComparison.OrdinalIgnoreCase))
                    {
                        bg = BadgeAmberBg;
                        fg = BadgeAmberText;
                    }
                    else
                    {
                        bg = BadgeGrayBg;
                        fg = BadgeGrayText;
                    }

                    var badgeRect = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + 8, Math.Min(e.CellBounds.Width - 16, 95), e.CellBounds.Height - 16);
                    using var brush = new SolidBrush(bg);
                    using var path = GetRoundedRectangle(badgeRect, 6);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);

                    TextRenderer.DrawText(e.Graphics, val, FontBold(8.5f), badgeRect, fg, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }
            }
        };
    }

    public static GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        int diameter = radius * 2;
        var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

        path.AddArc(arc, 180, 90);
        arc.X = bounds.Right - diameter;
        path.AddArc(arc, 270, 90);
        arc.Y = bounds.Bottom - diameter;
        path.AddArc(arc, 0, 90);
        arc.X = bounds.Left;
        path.AddArc(arc, 90, 90);
        path.CloseFigure();
        return path;
    }
}
