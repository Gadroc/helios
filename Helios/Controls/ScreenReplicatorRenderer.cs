//  Copyright 2014 Craig Courtney
//    
//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Helios is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace GadrocsWorkshop.Helios.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class ScreenReplicatorRenderer : HeliosVisualRenderer
    {
        private Rect _displayRect = new Rect(0, 0, 0, 0);
        private Brush _inactiveBrush;
        private Pen _inactivePen;
        private TextFormat _inactiveTextFormat;
        private IntPtr _hDC;
        private IntPtr _hMemDC;
        private IntPtr _hBitmap;
        private ImageBrush _screenBrush;

        public ScreenReplicatorRenderer()
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            ScreenReplicator replicator = Visual as ScreenReplicator;
            if (replicator != null)
            {                
                if (replicator.IsRunning)
                {
                    if (replicator.IsReplicating)
                    {
                        _screenBrush = CreateImageBrush(replicator);
                    }
                    else if (replicator.BlankOnStop)
                    {
                        _screenBrush = null;
                    }

                    if (_screenBrush != null)
                    {
                        drawingContext.DrawRectangle(_screenBrush, null, _displayRect);
                    }
                }
                else
                {
                    drawingContext.DrawRectangle(null, _inactivePen, _displayRect);
                    _inactiveTextFormat.RenderText(drawingContext, _inactiveBrush, "Screen Replicator", _displayRect);
                }
            }
        }

        protected override void OnRefresh()
        {
            ScreenReplicator replicator = Visual as ScreenReplicator;
            _inactiveBrush = Brushes.Yellow;
            _inactivePen = new Pen(_inactiveBrush, 1d);
            _inactivePen.DashStyle = DashStyles.Dash;
            _inactiveTextFormat = new TextFormat();
            _inactiveTextFormat.VerticalAlignment = TextVerticalAlignment.Center;
            _inactiveTextFormat.VerticalAlignment = TextVerticalAlignment.Center;

            if (replicator != null)
            {
                _displayRect.Width = replicator.Width;
                _displayRect.Height = replicator.Height;
            }
        }

        private ImageBrush CreateImageBrush(ScreenReplicator replicator)
        {
            ImageBrush brush = null;
            _hDC = NativeMethods.GetDC(NativeMethods.GetDesktopWindow());
            _hMemDC = NativeMethods.CreateCompatibleDC(_hDC);
            _hBitmap = NativeMethods.CreateCompatibleBitmap(_hDC, replicator.CaptureRectangle.Width, replicator.CaptureRectangle.Height);
            if (_hBitmap != IntPtr.Zero)
            {
                IntPtr hOld = (IntPtr)NativeMethods.SelectObject(_hMemDC, _hBitmap);

                NativeMethods.BitBlt(_hMemDC, 0, 0, replicator.CaptureRectangle.Width, replicator.CaptureRectangle.Height, _hDC,
                                           replicator.CaptureRectangle.X, replicator.CaptureRectangle.Y, NativeMethods.SRCCOPY);

                NativeMethods.SelectObject(_hMemDC, hOld);
                BitmapSource bmp = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(_hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(replicator.CaptureRectangle.Width, replicator.CaptureRectangle.Height));

                brush = new ImageBrush(bmp);
                NativeMethods.DeleteObject(_hBitmap);
            }
            NativeMethods.DeleteDC(_hMemDC);
            NativeMethods.ReleaseDC(NativeMethods.GetDesktopWindow(), _hDC);
            return brush;
        }
    }
}
