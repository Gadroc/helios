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

namespace GadrocsWorkshop.Helios
{
    using GadrocsWorkshop.Helios.Units;

    /// <summary>
    /// Helper class with all default unit constructors.
    /// </summary>
    public static class BindingValueUnits
    {        
        // Generic Units
        public static readonly NoValueUnit NoValue = new NoValueUnit();
        public static readonly NumericUnit Numeric = new NumericUnit();
        public static readonly TextUnit Text = new TextUnit();
        public static readonly BooleanUnit Boolean = new BooleanUnit();

        // Angle Units
        public static readonly RadiansUnit Radians = new RadiansUnit();
        public static readonly DegreesUnit Degrees = new DegreesUnit();

        // Temperature Units
        public static readonly CelsiusUnit Celsius = new CelsiusUnit();

        // Distance Units
        public static readonly MetersUnit Meters = new MetersUnit();
        public static readonly KilometersUnit Kilometers = new KilometersUnit();
        public static readonly FeetUnit Feet = new FeetUnit();
        public static readonly MilesUnit Miles = new MilesUnit();
        public static readonly NauticalMilesUnit NauticalMiles = new NauticalMilesUnit();

        // Revolutions Units
        public static readonly RPMPercentUnit RPMPercent = new RPMPercentUnit();
        public static RPMUnit RPM(double maxValue)
        {
            return new RPMUnit(maxValue);
        }

        // Time Units
        public static readonly SecondsUnit Seconds = new SecondsUnit();
        public static readonly MinuteUnit Minutes = new MinuteUnit();
        public static readonly HoursUnit Hours = new HoursUnit();

        // Mass Units
        public static readonly PoundsUnit Pounds = new PoundsUnit();
        public static readonly KilogramsUnit Kilograms = new KilogramsUnit();

        // Area Units
        public static readonly SquareInchUnit SquareInch = new SquareInchUnit();
        public static readonly SquareFootUnit SquareFoot = new SquareFootUnit();
        public static readonly SquareCentimeterUnit SquareCentimeter = new SquareCentimeterUnit();

        // Speed Units
        public static readonly BindingValueUnit MetersPerSecond = new SpeedUnit(Meters, Seconds, "m/s", "Meters per second");
        public static readonly BindingValueUnit FeetPerSecond = new SpeedUnit(Feet, Seconds, "fps", "Feet per second");
        public static readonly BindingValueUnit FeetPerMinute = new SpeedUnit(Feet, Minutes, "fpm", "Feet per minute");
        public static readonly BindingValueUnit MilesPerHour = new SpeedUnit(Miles, Hours, "mph", "Miles per hour");
        public static readonly BindingValueUnit Knots = new SpeedUnit(NauticalMiles, Hours, "kts", "Knots");
        public static readonly BindingValueUnit KilometersPerHour = new SpeedUnit(Kilometers, Hours, "km/h", "kilometers per hour");

        // Mass Flow Units
        public static readonly BindingValueUnit PoundsPerHour = new MassFlowUnit(Pounds, Hours, "PPH", "Pound per hour");

        // Pressure Units
        public static readonly BindingValueUnit PoundsPerSquareInch = new PressureUnit(Pounds, SquareInch, "PSI", "Pounds per square inch");
        public static readonly BindingValueUnit PoundsPerSquareFoot = new PressureUnit(Pounds, SquareFoot, "PSI", "Pounds per square foot");
        public static readonly BindingValueUnit InchesOfMercury = new InchofMercuryUnit();
        public static readonly BindingValueUnit MilimetersOfMercury = new MilimetersOfMercury();
        public static readonly BindingValueUnit KilgramsForcePerSquareCentimenter = new PressureUnit(Kilograms, SquareCentimeter, "kgf/cm2", "Kilograms Force per Square Centimeter");

        // Volume Units
        public static readonly BindingValueUnit Liters = new Liters();

        //Electrical Units
        public static readonly BindingValueUnit Volts = new NumericUnit();
    }
}
