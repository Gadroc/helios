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
    using System.Windows;
    using System.Windows.Media;

    public class GuardedThreeWayToggleRenderer : HeliosVisualRenderer
    {
        private ImageSource _imageOneGuardUp;
        private ImageSource _imageOneGuardDown;
        private ImageSource _imageTwoGuardUp;
        private ImageSource _imageTwoGuardDown;
        private ImageSource _imageThreeGuardUp;
        private ImageSource _imageThreeGuardDown;
        private Rect _imageRect;

        protected override void OnRender(DrawingContext drawingContext)
        {
            GuardedThreeWayToggle toggleSwitch = Visual as GuardedThreeWayToggle;
            if (toggleSwitch != null)
            {
                switch (toggleSwitch.SwitchPosition)
                {
                    case ThreeWayToggleSwitchPosition.One:
                        if (toggleSwitch.GuardPosition == GuardPosition.Up)
                        {
                            drawingContext.DrawImage(_imageOneGuardUp, _imageRect);
                        }
                        else
                        {
                            drawingContext.DrawImage(_imageOneGuardDown, _imageRect);
                        }
                        break;
                    case ThreeWayToggleSwitchPosition.Two:
                        if (toggleSwitch.GuardPosition == GuardPosition.Up)
                        {
                            drawingContext.DrawImage(_imageTwoGuardUp, _imageRect);
                        }
                        else
                        {
                            drawingContext.DrawImage(_imageTwoGuardDown, _imageRect);
                        }
                        break;
                    case ThreeWayToggleSwitchPosition.Three:
                        if (toggleSwitch.GuardPosition == GuardPosition.Up)
                        {
                            drawingContext.DrawImage(_imageThreeGuardUp, _imageRect);
                        }
                        else
                        {
                            drawingContext.DrawImage(_imageThreeGuardDown, _imageRect);
                        }
                        break;
                }
            }
        }

        protected override void OnRefresh()
        {
            GuardedThreeWayToggle toggleSwitch = Visual as GuardedThreeWayToggle;
            if (toggleSwitch != null)
            {
                _imageRect.Width = toggleSwitch.Width;
                _imageRect.Height = toggleSwitch.Height;
                _imageOneGuardUp = ConfigManager.ImageManager.LoadImage(toggleSwitch.PositionOneGuardUpImage);
                _imageOneGuardDown = ConfigManager.ImageManager.LoadImage(toggleSwitch.PositionOneGuardDownImage);
                _imageTwoGuardUp = ConfigManager.ImageManager.LoadImage(toggleSwitch.PositionTwoGuardUpImage);
                _imageTwoGuardDown = ConfigManager.ImageManager.LoadImage(toggleSwitch.PositionTwoGuardDownImage);
                _imageThreeGuardUp = ConfigManager.ImageManager.LoadImage(toggleSwitch.PositionThreeGuardUpImage);
                _imageThreeGuardDown = ConfigManager.ImageManager.LoadImage(toggleSwitch.PositionThreeGuardDownImage);
            }
            else
            {
                _imageOneGuardUp = null;
                _imageOneGuardDown = null;
                _imageTwoGuardUp = null;
                _imageTwoGuardDown = null;
                _imageThreeGuardUp = null;
                _imageThreeGuardDown = null;
            }
        }
    }
}
