using System;

class WeighingMachine
{
    private double _weight;
    private int _precision;

    public WeighingMachine(int precision)
    {
        Precision = precision;
    }

    public int Precision
    {
        get { return _precision; }
        private set
        {
            _precision = value;
        }
    }

    public double TareAdjustment { get; set; } = 5.0;

    public double Weight
    {
        get
        {
            return _weight;
        }
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException();
            _weight = value;
        }
    }

    public string DisplayWeight
    {
        get
        {
            return Math.Round(Weight - TareAdjustment, Precision).ToString($"F{Precision}") + " kg";
        }
    }
}
