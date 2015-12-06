using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SteppedMotor.RedPlate
{
    class Program
    {

        private static  CancellationTokenSource _сancellationToken;
        private static short _decData;
        private static short _disData;
        private static int _delay = 1;
        private static int _activeDelay = 1;
        private static char[] _cmd = new char[] {'0', '0', '0', '0', '0', '0', '0', '0', };
        private static char[] _disCmd = new char[] { '0', '0', '0', '0', '0', '0', '0', '0', };
        private static bool _pause;

        static void Main(string[] args)
        {
            _сancellationToken = new CancellationTokenSource();
            _decData = (short)Convert.ToInt32(new string(_cmd), 2);
            _disData = (short)Convert.ToInt32(new string(_disCmd), 2);
            var sendTask = Task.Factory.StartNew(SendData);
            Task.Factory.StartNew(ReadInput);
            Console.WriteLine("1");
            sendTask.Wait();
        }

        private static void ReadInput()
        {
            while (!_сancellationToken.IsCancellationRequested)
            {
                var key = Console.ReadKey(true);
                _pause = true;
                switch (key.Key)
                {
                    case ConsoleKey.F1:
                        _cmd[7] = _cmd[7] == '1' ? '0' : '1';
                        _decData = (short) Convert.ToInt32(new string(_cmd), 2);
                        Console.Clear();
                        Console.WriteLine(_cmd);
                        break;
                    case ConsoleKey.F2:
                        _cmd[6] = _cmd[6] == '1' ? '0' : '1';
                        _disCmd[6] = _cmd[6];
                        _decData = (short) Convert.ToInt32(new string(_cmd), 2);
                        _disData = (short)Convert.ToInt32(new string(_disCmd), 2);
                        Console.Clear();
                        Console.WriteLine(_cmd);
                        break;
                    case ConsoleKey.F3:
                        _cmd[5] = _cmd[5] == '1' ? '0' : '1';
                        _decData = (short)Convert.ToInt32(new string(_cmd), 2);
                        Console.Clear();
                        Console.WriteLine(_cmd);
                        break;
                    case ConsoleKey.F4:
                        _cmd[4] = _cmd[4] == '1' ? '0' : '1';
                        _disCmd[4] = _cmd[4];
                        _decData = (short)Convert.ToInt32(new string(_cmd), 2);
                        _disData = (short)Convert.ToInt32(new string(_disCmd), 2);
                        Console.Clear();
                        Console.WriteLine(_cmd);
                        break;
                    case ConsoleKey.NumPad1:
                        _delay++;
                        break;
                    case ConsoleKey.NumPad2:
                        if (_delay > 1)
                            _delay--;
                        break;
                    case ConsoleKey.NumPad3:
                        _activeDelay++;
                        break;
                    case ConsoleKey.NumPad4:
                        if (_activeDelay > 1)
                            _activeDelay--;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (_delay > 1)
                            _delay--;
                        break;
                    case ConsoleKey.Escape:
                        _сancellationToken.Cancel();
                        break;

                }
                _pause = false;
            }

        }

        private static void SendData()
        {
            
            short decAdd = 0x378; // 378h Selected Default
            while (!_сancellationToken.IsCancellationRequested)
            {
                if (_pause)
                    continue;
                //decData =1;
                PortAccess.Output(decAdd, _decData);
                Thread.Sleep(_activeDelay);
                //decData = Convert.ToByte("00000000", 2);
                PortAccess.Output(decAdd, _disData);
                //var data =PortAccess.Input(decAdd);
                //Console.WriteLine(data);
                Thread.Sleep(_delay);

            }
            PortAccess.Output(decAdd, 0);
        }
    }
}
