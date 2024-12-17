using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main
{
    public static class TouchesGlobales
    {
        public static Key ToucheHaut { get; set; } = Key.Z;
        public static Key ToucheBas { get; set; } = Key.S;
        public static Key ToucheGauche { get; set; } = Key.Q;
        public static Key ToucheDroite { get; set; } = Key.D;
        public static Key ToucheActiver { get; set; } = Key.E;
    }
}