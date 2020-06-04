using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


/// <summary>
/// Пространство плагина для управления игровыми персонажами
/// на основе исскуственных сетей
/// </summary>
namespace NeuralNetworkPlugin
{
    #region ArtificiaNeuralNetwork

    /// <summary>
    /// Класс "Нейрон".
    /// </summary>
    public class Neuron
    {
        /// <summary>
        /// Значение нейрона.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Пустой конструктор класса.
        /// </summary>
        public Neuron()
        {
            Value = 0f;
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Value">Значение нейрона</param>
        public Neuron(float _Value) 
        {  
            Value = _Value; 
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Value">Значение нейрона</param>
        public Neuron(double _Value) 
        {
            Value = Convert.ToSingle(_Value); 
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Value">Значение нейрона</param>
        public Neuron(int _Value)
        {
            Value = Convert.ToSingle(_Value);
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Value">Значение нейрона</param>
        public Neuron(string _Value)
        {
            Value = Convert.ToSingle(_Value);
        }
        
        /// <summary>
        /// Деструктор класса.
        /// </summary>
        ~Neuron() { }

        /// <summary>
        /// Паттерн - Прототип. 
        /// Клонирование объекта прототипа.
        /// </summary>
        /// <returns>Клон объекта</returns>
        public Neuron Clone()
        {
            Neuron clone = (Neuron)MemberwiseClone();
            clone.Value = Value;

            return clone;
        }

        /// <summary>
        /// Переопределение оператора == .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator ==(Neuron objF, Neuron objS)
        {
            if (objF.Value != objS.Value)
                return false;
            return true;
        }

        /// <summary>
        /// Переопределение оператора != .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator !=(Neuron objF, Neuron objS)
        {
            if (objF.Value == objS.Value)
                return false;
            return true;
        }

        /// <summary>
        /// Переопределение оператора < .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator >(Neuron objF, Neuron objS)
        {
            if (objF.Value <= objS.Value)
                return false;
            return true;
        }

        /// <summary>
        /// Переопределение оператора > .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator <(Neuron objF, Neuron objS)
        {
            if (objF.Value >= objS.Value)
                return false;
            return true;
        }

        /// <summary>
        /// Переопределение оператора <= .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator >=(Neuron objF, Neuron objS)
        {
            if (objF.Value < objS.Value)
                return false;
            return true;
        }

        /// <summary>
        /// Переопределение оператора >= .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator <=(Neuron objF, Neuron objS)
        {
            if (objF.Value > objS.Value)
                return false;
            return true;
        }

        /// <summary>
        /// Переопределение Equals.
        /// </summary>
        ///<param name="obj">Первый объект</param>
        public override bool Equals(object obj)
        {
            return obj is Neuron neuron && Value == neuron.Value;
        }

        /// <summary>
        /// Переопределение GetHashCode.
        /// </summary>
        public override int GetHashCode()
        {
            return -1937169414 + Value.GetHashCode();
        }
    }

    /// <summary>
    /// Класс "Слой"
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Нейроны слоя
        /// </summary>
        public List<Neuron> Neurons { get; set; }

        /// <summary>
        /// Пустой конструктор класса.
        /// </summary>
        public Layer()
        {
            Neurons = new List<Neuron>();
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Neurons">Список нейронов</param>
        public Layer(List<Neuron> _Neurons)
        {
            Neurons = new List<Neuron>();

            foreach (Neuron neuron in _Neurons)
               Neurons.Add(neuron.Clone());
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Values">Список значений нейронов</param>
        public Layer(List<float> _Values)
        {
            Neurons = new List<Neuron>();

            foreach (float value in _Values)
                Neurons.Add(new Neuron(value));
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Values">Список значений нейронов</param>
        public Layer(List<double> _Values)
        {
            Neurons = new List<Neuron>();

            foreach (double value in _Values)
                Neurons.Add(new Neuron(value));
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Values">Список значений нейронов</param>
        public Layer(List<int> _Values)
        {
            Neurons = new List<Neuron>();

            foreach (int value in _Values)
                Neurons.Add(new Neuron(value));
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Values">Список значений нейронов</param>
        public Layer(List<string> _Values)
        {
            Neurons = new List<Neuron>();

            foreach (string value in _Values)
                Neurons.Add(new Neuron(value));
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Values">Массив значений нейронов</param>
        public Layer(float[] _Values)
        {
            Neurons = new List<Neuron>();

            foreach (float value in _Values)
                Neurons.Add(new Neuron(value));
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Values">Массив значений нейронов</param>
        public Layer(double[] _Values)
        {
            Neurons = new List<Neuron>();

            foreach (double value in _Values)
                Neurons.Add(new Neuron(value));
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Values">Массив значений нейронов</param>
        public Layer(int[] _Values)
        {
            Neurons = new List<Neuron>();

            foreach (int value in _Values)
                Neurons.Add(new Neuron(value));
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Values">Массив значений нейронов</param>
        public Layer(string[] _Values)
        {
            Neurons = new List<Neuron>();

            foreach (string value in _Values)
                Neurons.Add(new Neuron(value));
        }

        /// <summary>
        /// Деструктор класса.
        /// </summary>
        ~Layer() { }

        /// <summary>
        /// Паттерн - Прототип. 
        /// Клонирование объекта прототипа.
        /// </summary>
        /// <returns>Клон объекта</returns>
        public Layer Clone()
        {
            Layer clone = (Layer)MemberwiseClone();

            clone.Neurons = new List<Neuron>();
            for(int i = 0; i < Neurons.Count; i++)
                clone.Neurons.Add(new Neuron(Neurons[i].Value));

            return clone;
        }

        /// <summary>
        /// Переопределение оператора == .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator ==(Layer objF, Layer objS)
        {
            for (int i = 0; i < objF.Neurons.Count; i++)
                for (int j = 0; j < objS.Neurons.Count; j++)
                    if (objF.Neurons[i] != objS.Neurons[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора != .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator !=(Layer objF, Layer objS)
        {
            for(int i = 0; i < objF.Neurons.Count; i++)
                for (int j = 0; j < objS.Neurons.Count; j++)
                    if (objF.Neurons[i] == objS.Neurons[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора < .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator >(Layer objF, Layer objS)
        {
            for (int i = 0; i < objF.Neurons.Count; i++)
                for (int j = 0; j < objS.Neurons.Count; j++)
                    if (objF.Neurons[i] <= objS.Neurons[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора > .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator <(Layer objF, Layer objS)
        {
            for (int i = 0; i < objF.Neurons.Count; i++)
                for (int j = 0; j < objS.Neurons.Count; j++)
                    if (objF.Neurons[i] >= objS.Neurons[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора <= .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator >=(Layer objF, Layer objS)
        {
            for (int i = 0; i < objF.Neurons.Count; i++)
                for (int j = 0; j < objS.Neurons.Count; j++)
                    if (objF.Neurons[i] < objS.Neurons[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора >= .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator <=(Layer objF, Layer objS)
        {
            for (int i = 0; i < objF.Neurons.Count; i++)
                for (int j = 0; j < objS.Neurons.Count; j++)
                    if (objF.Neurons[i] > objS.Neurons[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение Equals.
        /// </summary>
        ///<param name="obj">Первый объект</param>
        public override bool Equals(object obj)
        {
            return obj is Layer layer &&
                   EqualityComparer<List<Neuron>>.Default.Equals(Neurons, layer.Neurons);
        }

        /// <summary>
        /// Переопределение GetHashCode.
        /// </summary>
        public override int GetHashCode()
        {
            return 368358039 + EqualityComparer<List<Neuron>>.Default.GetHashCode(Neurons);
        }
    }

    /// <summary>
    /// Класс "Синапс".
    /// </summary>
    public class Synapse
    {
        /// <summary>
        /// Первый нейрон синапса, входной нейрон синапса.
        /// </summary>
        public Neuron Source { get; set; }

        /// <summary>
        /// Второй нейрон синапса, выходной 
        /// </summary>
        public Neuron Outlet { get; set; }

        /// <summary>
        /// Вес синапса.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Пустой конструктор класса.
        /// </summary>
        public Synapse()
        {
            Source = new Neuron();
            Outlet = new Neuron();
            Value = 0f;
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Source">Входной нейрон</param>
        /// <param name="_Outlet">Выходной нейрон</param>
        /// <param name="_Value">Вес синапса</param>
        public Synapse(Neuron _Source, Neuron _Outlet, float _Value)
        {
            Source = _Source.Clone();
            Outlet = _Outlet.Clone();
            Value = _Value;
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Source">Входной нейрон</param>
        /// <param name="_Outlet">Выходной нейрон</param>
        /// <param name="_Value">Вес синапса</param>
        public Synapse(Neuron _Source, Neuron _Outlet, double _Value)
        {
            Source = _Source.Clone();
            Outlet = _Outlet.Clone();
            Value = Convert.ToSingle(_Value);
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Source">Входной нейрон</param>
        /// <param name="_Outlet">Выходной нейрон</param>
        /// <param name="_Value">Вес синапса</param>
        public Synapse(Neuron _Source, Neuron _Outlet, int _Value)
        {
            Source = _Source.Clone();
            Outlet = _Outlet.Clone();
            Value = Convert.ToSingle(_Value);
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Source">Входной нейрон</param>
        /// <param name="_Outlet">Выходной нейрон</param>
        /// <param name="_Value">Вес синапса</param>
        public Synapse(Neuron _Source, Neuron _Outlet, string _Value)
        {
            Source = _Source.Clone();
            Outlet = _Outlet.Clone();
            Value = Convert.ToSingle(_Value);
        }

        /// <summary>
        /// Деструктор класса.
        /// </summary>
        ~Synapse() { }

        /// <summary>
        /// Паттерн - Прототип. 
        /// Клонирование объекта прототипа.
        /// </summary>
        /// <returns>Клон объекта</returns>
        public Synapse Clone()
        {
            Synapse clone = (Synapse)MemberwiseClone();

            clone.Source = new Neuron(Source.Value);
            clone.Outlet = new Neuron(Outlet.Value);
            clone.Value = Value;

            return clone;
        }

        /// <summary>
        /// Переопределение оператора == .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator ==(Synapse objF, Synapse objS)
        {
            if (objF.Source != objS.Source)
                return false;

            if (objF.Outlet != objS.Outlet)
                return false;

            if (objF.Value != objS.Value)
                return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора != .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator !=(Synapse objF, Synapse objS)
        {
            if (objF.Source == objS.Source)
                return false;

            if (objF.Outlet == objS.Outlet)
                return false;

            if (objF.Value == objS.Value)
                return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора < .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator >(Synapse objF, Synapse objS)
        {
            if (objF.Source <= objS.Source)
                return false;

            if (objF.Outlet <= objS.Outlet)
                return false;

            if (objF.Value <= objS.Value)
                return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора > .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator <(Synapse objF, Synapse objS)
        {
            if (objF.Source >= objS.Source)
                return false;

            if (objF.Outlet >= objS.Outlet)
                return false;

            if (objF.Value >= objS.Value)
                return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора <= .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator >=(Synapse objF, Synapse objS)
        {
            if (objF.Source < objS.Source)
                return false;

            if (objF.Outlet < objS.Outlet)
                return false;

            if (objF.Value < objS.Value)
                return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора >= .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator <=(Synapse objF, Synapse objS)
        {
            if (objF.Source > objS.Source)
                return false;

            if (objF.Outlet > objS.Outlet)
                return false;

            if (objF.Value > objS.Value)
                return false;

            return true;
        }

        /// <summary>
        /// Переопределение Equals.
        /// </summary>
        ///<param name="obj">Первый объект</param>
        public override bool Equals(object obj)
        {
            return obj is Synapse synapse &&
                   EqualityComparer<Neuron>.Default.Equals(Source, synapse.Source) &&
                   EqualityComparer<Neuron>.Default.Equals(Outlet, synapse.Outlet) &&
                   Value == synapse.Value;
        }

        /// <summary>
        /// Переопределение GetHashCode.
        /// </summary>
        public override int GetHashCode()
        {
            int hashCode = 1853232146;
            hashCode = hashCode * -1521134295 + EqualityComparer<Neuron>.Default.GetHashCode(Source);
            hashCode = hashCode * -1521134295 + EqualityComparer<Neuron>.Default.GetHashCode(Outlet);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    /// Класс "Искусственная нейронная сеть"
    /// </summary>
    public class ArtificiaNeuralNetwork
    {
        /// <summary>
        /// Слои ИНС.
        /// </summary>
        public List<Layer> Layers { get; set; }

        /// <summary>
        /// Синапсы ИНС.
        /// </summary>
        public List<Synapse> Synapses { get; set; }

        /// <summary>
        /// Пустой конструктор класса.
        /// </summary>
        public ArtificiaNeuralNetwork()
        {
            Layers = new List<Layer>();
            Synapses = new List<Synapse>();
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Layers">Список слоев</param>
        /// <param name="_Synapses">Список синапсов</param>
        public ArtificiaNeuralNetwork(List<Layer> _Layers, List<Synapse> _Synapses)
        {
            Layers = new List<Layer>();
            for (int i = 0; i < _Layers.Count; i++)
                Layers.Add(_Layers[i].Clone());

            Synapses = new List<Synapse>();
            for (int i = 0; i < _Synapses.Count; i++)
                Synapses.Add(_Synapses[i].Clone());
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Layers">Список слоев</param>
        /// <param name="_Synapses">Список синапсов</param>
        public ArtificiaNeuralNetwork(Layer[] _Layers, List<Synapse> _Synapses)
        {
            Layers = new List<Layer>();
            for (int i = 0; i < _Layers.Length; i++)
                Layers.Add(_Layers[i].Clone());

            Synapses = new List<Synapse>();
            for (int i = 0; i < _Synapses.Count; i++)
                Synapses.Add(_Synapses[i].Clone());
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Layers">Список слоев</param>
        /// <param name="_Synapses">Список синапсов</param>
        public ArtificiaNeuralNetwork(List<Layer> _Layers, Synapse[] _Synapses)
        {
            Layers = new List<Layer>();
            for (int i = 0; i < _Layers.Count; i++)
                Layers.Add(_Layers[i].Clone());

            Synapses = new List<Synapse>();
            for (int i = 0; i < _Synapses.Length; i++)
                Synapses.Add(_Synapses[i].Clone());
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Layers">Список слоев</param>
        /// <param name="_Synapses">Список синапсов</param>
        public ArtificiaNeuralNetwork(Layer[] _Layers, Synapse[] _Synapses)
        {
            Layers = new List<Layer>();
            for (int i = 0; i < _Layers.Length; i++)
                Layers.Add(_Layers[i].Clone());

            Synapses = new List<Synapse>();
            for (int i = 0; i < _Synapses.Length; i++)
                Synapses.Add(_Synapses[i].Clone());
        }

        /// <summary>
        /// Инициализация нейронов.
        /// </summary>
        /// <param name="_Layers">Слои искусственной нейронной сети</param>
        public void InitNeurons(List<int> _Layers)
        {
            for (int i = 0; i < _Layers.Count; i++)
                Layers.Add(new Layer());

            for (int i = 0; i < _Layers.Count; i++)
            {
                Layers[i].Neurons = new List<Neuron>();
                for (int j = 0; j < _Layers[i]; j++)
                    Layers[i].Neurons.Add(new Neuron());
            }
        }

        /// <summary>
        /// Инициализация нейронов.
        /// </summary>
        /// <param name="_Layers">Слои искусственной нейронной сети</param>
        public void InitNeurons(int[] _Layers)
        {
            for (int i = 0; i < _Layers.Length; i++)
                Layers.Add(new Layer());

            for (int i = 0; i < _Layers.Length; i++)
            {
                Layers[i].Neurons = new List<Neuron>();
                for (int j = 0; j < _Layers[i]; j++)
                    Layers[i].Neurons.Add(new Neuron());
            }
        }

        /// <summary>
        /// Инициализация синапсов со случайным весом.
        /// </summary>
        public void InitSynapses()
        {
            for (int i = 0; i < Layers.Count; i++)
                for (int j = 0; j < Layers[i].Neurons.Count; j++)
                    if (i > 0)
                        for (int k = 0; k < Layers[i - 1].Neurons.Count; k++)
                            Synapses.Add(new Synapse(Layers[i - 1].Neurons[k], Layers[i].Neurons[j], UnityEngine.Random.Range(-0.5f, 0.5f)));
        }

        /// <summary>
        /// Основная работа искусственной нейронной сети.
        /// </summary>
        public Layer WorkNeurons(List<float> _InputData, string _NameActivationFunction)
        {
            for (int i = 0; i < Layers.Count; i++)
                for (int j = 0; j < Layers[i].Neurons.Count; j++)
                {
                    // данные входного слоя
                    if (i == 0)
                        Layers[i].Neurons[j].Value = _InputData[j];
                    // данные срытых и выходного слоев
                    else
                    {
                        float temp_value = 0f;
                        Layers[i].Neurons[j].Value = 0f;

                        for (int k = 0; k < Layers[i - 1].Neurons.Count; k++)
                            temp_value += Layers[i - 1].Neurons[k].Value * Synapses[i + j + k].Value;

                        Layers[i].Neurons[j].Value = UseFunction(_NameActivationFunction, temp_value);
                    }
                }

            return Layers.Last();
        }

        /// <summary>
        /// Основная работа искусственной нейронной сети.
        /// </summary>
        public Layer WorkNeurons(float[] _InputData, string _NameActivationFunction)
        {
            for (int i = 0; i < Layers.Count; i++)
                for (int j = 0; j < Layers[i].Neurons.Count; j++)
                {
                    // данные входного слоя
                    if (i == 0)
                        Layers[i].Neurons[j].Value = _InputData[j];
                    // данные срытых и выходного слоев
                    else
                    {
                        float temp_value = 0f;
                        Layers[i].Neurons[j].Value = 0f;

                        for (int k = 0; k < Layers[i - 1].Neurons.Count; k++)
                            temp_value += Layers[i - 1].Neurons[k].Value * Synapses[i + j + k].Value;

                        Layers[i].Neurons[j].Value = UseFunction(_NameActivationFunction, temp_value);
                    }
                }

            return Layers.Last();
        }

        /// <summary>
        /// Использование функции активации.
        /// </summary>
        /// <param name="_NameActivationFuction">Имя функции: Linear, Tanh, Sigmoid, ReLU,
        /// Threshold, Iconic, RadialBase, SemilinearS, LinearS, Triangular</param>
        /// <returns></returns>
        public float UseFunction(string _NameActivationFuction, float _Value)
        {
            if (_NameActivationFuction == "Linear")
                return new ActivationFunction(new LinearFunction()).Calculate(_Value);

            if (_NameActivationFuction == "Tanh")
                return new ActivationFunction(new HyperbolicTangentFunction()).Calculate(_Value);

            if (_NameActivationFuction == "Sigmoid")
                return new ActivationFunction(new SigmoidFunction()).Calculate(_Value);

            if(_NameActivationFuction == "ReLU")
                return new ActivationFunction(new ReLUFunction()).Calculate(_Value);

            if (_NameActivationFuction == "Threshold")
                return new ActivationFunction(new ThresholdFunction()).Calculate(_Value);

            if (_NameActivationFuction == "Iconic")
                return new ActivationFunction(new IconicFunction()).Calculate(_Value);

            if (_NameActivationFuction == "RadialBase")
                return new ActivationFunction(new RadialBaseFunction()).Calculate(_Value);

            if (_NameActivationFuction == "SemilinearS")
                return new ActivationFunction(new SemilinearSaturationFunction()).Calculate(_Value);

            if (_NameActivationFuction == "LinearS")
                return new ActivationFunction(new LinearSaturationFunction()).Calculate(_Value);

            if (_NameActivationFuction == "Triangular")
                return new ActivationFunction(new TriangularFunction()).Calculate(_Value);

            return 0f;
        }

        /// <summary>
        /// Конвертация состояния нейронной сети в строку
        /// для записи в файл.
        /// </summary>
        /// <returns>string</returns>
        private string SetStringToFile()
        {
            string ANN = "";

            for(int i = 0; i < Layers.Count; i++)
            {
                if (i < Layers.Count - 1)
                {
                    for (int j = 0; j < Layers[i].Neurons.Count; j++)
                    {
                        if (j < Layers[i].Neurons.Count - 1)
                            ANN += Layers[i].Neurons[j].Value.ToString() + "/";
                        else
                            ANN += Layers[i].Neurons[j].Value.ToString();
                    }
                    ANN += "*";
                }
                else
                {
                    for (int j = 0; j < Layers[i].Neurons.Count; j++)
                    {
                        if (j < Layers[i].Neurons.Count - 1)
                            ANN += Layers[i].Neurons[j].Value.ToString() + "/";
                        else
                            ANN += Layers[i].Neurons[j].Value.ToString();
                    }
                }
            }

            ANN += "$";

            for(int i = 0; i < Synapses.Count; i++)
            {
                if(i < Synapses.Count - 1)
                    ANN += Synapses[i].Source.Value.ToString() + ":" + Synapses[i].Outlet.Value.ToString() + ":" + Synapses[i].Value.ToString() + "~";
                else
                    ANN += Synapses[i].Source.Value.ToString() + ":" + Synapses[i].Outlet.Value.ToString() + ":" + Synapses[i].Value.ToString();
            }

            return ANN;
        }

        /// <summary>
        /// Конвертация состояния нейронной сети
        /// из файла в строку.
        /// </summary>
        /// <returns></returns>
        private string GetStrngFromFile()
        {
            string Text;
            StreamReader Reader = new StreamReader(Directory.GetCurrentDirectory() + $"/SaveNeuralNetwork.bin");
            Text = Reader.ReadLine();
            Reader.Close();

            return Text;
        }

        /// <summary>
        /// Сохранение нейронной сети в файл.
        /// </summary>
        public void SaveNeuralNetwork()
        {
            StreamWriter Writer = new StreamWriter(Directory.GetCurrentDirectory() + $"/SaveNeuralNetwork.bin");
            Writer.WriteLine(this.SetStringToFile());
            Writer.Close();
        }

        /// <summary>
        /// Загрузка нейронной сети.
        /// Многослойный персептрон.
        /// </summary>
        /// <returns></returns>
        public MultilayerPerceptron LoadNeuralNetwork()
        {
            List<Layer> new_layers = new List<Layer>();
            List<Synapse> new_synapses = new List<Synapse>();

            string[] parts = GetStrngFromFile().Split('$');

            string[] layers = parts[0].Split('*');
            string[][] neurons = new string[layers.Length][];

            for (int i = 0; i < layers.Length; i++)
                neurons[i] = layers[i].Split('/');

            string[] synapses = parts[1].Split('~');
            string[][] synapses_full = new string[synapses.Length][];

            for (int i = 0; i < synapses.Length; i++)
                synapses_full[i] = synapses[i].Split(':');

            for (int i = 0; i < layers.Length; i++)
            {
                List<Neuron> new_neurons = new List<Neuron>();
                for (int j = 0; j < neurons[i].Length; j++)
                    new_neurons.Add(new Neuron(float.Parse(neurons[i][j])));
                new_layers.Add(new Layer(new_neurons));
            }

            for (int i = 0; i < synapses.Length; i++)
            {
                Neuron source = new Neuron(float.Parse(synapses_full[i][0]));
                Neuron outlet = new Neuron(float.Parse(synapses_full[i][1]));
                new_synapses.Add(new Synapse(source, outlet, float.Parse(synapses_full[i][2])));
            }

            return new MultilayerPerceptron(new_layers, new_synapses);
        }

        /// <summary>
        /// Деструктор класса.
        /// </summary>
        ~ArtificiaNeuralNetwork() { }

        /// <summary>
        /// Паттерн - Прототип. 
        /// Клонирование объекта прототипа.
        /// </summary>
        /// <returns>Клон объекта</returns>
        public ArtificiaNeuralNetwork Clone()
        {
            ArtificiaNeuralNetwork clone = (ArtificiaNeuralNetwork)MemberwiseClone();

            clone.Layers = new List<Layer>();
            for (int i = 0; i < Layers.Count; i++)
                clone.Layers.Add(Layers[i].Clone());

            clone.Synapses = new List<Synapse>();
            for (int i = 0; i < Synapses.Count; i++)
                clone.Synapses.Add(Synapses[i].Clone());

            return clone;
        }

        /// <summary>
        /// Переопределение оператора == .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator ==(ArtificiaNeuralNetwork objF, ArtificiaNeuralNetwork objS)
        {
            for (int i = 0; i < objF.Layers.Count; i++)
                for (int j = 0; j < objS.Layers.Count; j++)
                    if (objF.Layers[i] != objS.Layers[j])
                        return false;

            for (int i = 0; i < objF.Synapses.Count; i++)
                for (int j = 0; j < objS.Synapses.Count; j++)
                    if (objF.Synapses[i] != objS.Synapses[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора != .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator !=(ArtificiaNeuralNetwork objF, ArtificiaNeuralNetwork objS)
        {
            for (int i = 0; i < objF.Layers.Count; i++)
                for (int j = 0; j < objS.Layers.Count; j++)
                    if (objF.Layers[i] == objS.Layers[j])
                        return false;

            for (int i = 0; i < objF.Synapses.Count; i++)
                for (int j = 0; j < objS.Synapses.Count; j++)
                    if (objF.Synapses[i] == objS.Synapses[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора < .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator >(ArtificiaNeuralNetwork objF, ArtificiaNeuralNetwork objS)
        {
            for (int i = 0; i < objF.Layers.Count; i++)
                for (int j = 0; j < objS.Layers.Count; j++)
                    if (objF.Layers[i] <= objS.Layers[j])
                        return false;

            for (int i = 0; i < objF.Synapses.Count; i++)
                for (int j = 0; j < objS.Synapses.Count; j++)
                    if (objF.Synapses[i] <= objS.Synapses[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора > .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator <(ArtificiaNeuralNetwork objF, ArtificiaNeuralNetwork objS)
        {
            for (int i = 0; i < objF.Layers.Count; i++)
                for (int j = 0; j < objS.Layers.Count; j++)
                    if (objF.Layers[i] >= objS.Layers[j])
                        return false;

            for (int i = 0; i < objF.Synapses.Count; i++)
                for (int j = 0; j < objS.Synapses.Count; j++)
                    if (objF.Synapses[i] >= objS.Synapses[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора <= .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator >=(ArtificiaNeuralNetwork objF, ArtificiaNeuralNetwork objS)
        {
            for (int i = 0; i < objF.Layers.Count; i++)
                for (int j = 0; j < objS.Layers.Count; j++)
                    if (objF.Layers[i] < objS.Layers[j])
                        return false;

            for (int i = 0; i < objF.Synapses.Count; i++)
                for (int j = 0; j < objS.Synapses.Count; j++)
                    if (objF.Synapses[i] < objS.Synapses[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение оператора >= .
        /// </summary>
        /// <param name="objF">Первый объект</param>
        /// <param name="objS">Второй объект</param>
        public static bool operator <=(ArtificiaNeuralNetwork objF, ArtificiaNeuralNetwork objS)
        {
            for (int i = 0; i < objF.Layers.Count; i++)
                for (int j = 0; j < objS.Layers.Count; j++)
                    if (objF.Layers[i] > objS.Layers[j])
                        return false;

            for (int i = 0; i < objF.Synapses.Count; i++)
                for (int j = 0; j < objS.Synapses.Count; j++)
                    if (objF.Synapses[i] > objS.Synapses[j])
                        return false;

            return true;
        }

        /// <summary>
        /// Переопределение Equals.
        /// </summary>
        ///<param name="obj">Первый объект</param>
        public override bool Equals(object obj)
        {
            return obj is ArtificiaNeuralNetwork network &&
                   EqualityComparer<List<Layer>>.Default.Equals(Layers, network.Layers) &&
                   EqualityComparer<List<Synapse>>.Default.Equals(Synapses, network.Synapses);
        }

        /// <summary>
        /// Переопределение GetHashCode.
        /// </summary>
        public override int GetHashCode()
        {
            int hashCode = -589887192;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Layer>>.Default.GetHashCode(Layers);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Synapse>>.Default.GetHashCode(Synapses);
            return hashCode;
        }
    }

    /// <summary>
    /// Класс "Многослойный пересптрон". 
    /// Нейронная сеть прямого распространения с несколькими скрытыми слоями.
    /// </summary>
    public class MultilayerPerceptron : ArtificiaNeuralNetwork
    {
        /// <summary>
        /// Пустой конструктор класса.
        /// </summary>
        public MultilayerPerceptron()
        {
            Layers = new List<Layer>();
            Synapses = new List<Synapse>();
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Layers">Слои ИНС</param>
        /// <param name="_Synapses">Синапсы ИНС</param>
        public MultilayerPerceptron(List<Layer> _Layers, List<Synapse> _Synapses)
        {
            Layers = new List<Layer>();
            for (int i = 0; i < _Layers.Count; i++)
                Layers.Add(_Layers[i].Clone());

            Synapses = new List<Synapse>();
            for (int i = 0; i < _Synapses.Count; i++)
                Synapses.Add(_Synapses[i].Clone());
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_ArtificiaNeuralNetwork">Нейронная сеть</param>
        public MultilayerPerceptron(MultilayerPerceptron _ArtificiaNeuralNetwork)
        {      
            Layers = new List<Layer>();
            for (int i = 0; i < _ArtificiaNeuralNetwork.Layers.Count; i++)
                Layers.Add(_ArtificiaNeuralNetwork.Layers[i].Clone());

            Synapses = new List<Synapse>();
            for (int i = 0; i < _ArtificiaNeuralNetwork.Synapses.Count; i++)
                Synapses.Add(_ArtificiaNeuralNetwork.Synapses[i].Clone());
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Layers">Слои искусственной нейронной сети</param>
        public MultilayerPerceptron(List<Layer> _Layers)
        {
            Layers = new List<Layer>();
            for (int i = 0; i < _Layers.Count; i++)
                Layers.Add(_Layers[i].Clone());

            Synapses = new List<Synapse>();
            InitSynapses();
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Layers">Слои искусственной нейронной сети</param>
        public MultilayerPerceptron(List<int> _Layers)
        {
            Layers = new List<Layer>();
            Synapses = new List<Synapse>();

            InitNeurons(_Layers);
            InitSynapses();
        }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Layers">Слои искусственной нейронной сети</param>
        public MultilayerPerceptron(int[] _Layers)
        {
            Layers = new List<Layer>();
            Synapses = new List<Synapse>();

            InitNeurons(_Layers);
            InitSynapses();
        }

        /// <summary>
        /// Обучение многослойного перцептрона без учителя.
        /// </summary>
        /// <param name="_ArtificiaNeuralNetwork">Входная нейронная сеть</param>
        /// <param name="_ProbabilityChange">Шанс изменения</param>
        /// <returns>Обученая нейронная сеть</returns>
        public MultilayerPerceptron UseTrainingSystem(MultilayerPerceptron _ArtificiaNeuralNetwork, float _ProbabilityChange)
        {
            return new TrainingSystem(new TeacherlessLearning()).Training(_ArtificiaNeuralNetwork, _ProbabilityChange);
        }

        /// <summary>
        /// Обучение многослойного перцептрона без учителя.
        /// </summary>
        /// <param name="_ProbabilityChange">Шанс изменения</param>
        /// <returns>Обученая нейронная сеть</returns>
        public MultilayerPerceptron UseTrainingSystem(float _ProbabilityChange)
        {
            return new TrainingSystem(new TeacherlessLearning()).Training(this, _ProbabilityChange);
        }
    }

    #endregion

    #region ActivationFunction

    /// <summary>
    /// Класс "Функция активации". 
    /// </summary>
    public class ActivationFunction
    {
        private IActivationFunction Function;

        /// <summary>
        /// Пустой Конструктор класса.
        /// </summary>
        public ActivationFunction() { }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="_Function"></param>
        public ActivationFunction(IActivationFunction _Function)
        {
            SetFunction(_Function);
        }

        /// <summary>
        /// Деструктор класса.
        /// </summary>
        ~ActivationFunction() { }

        /// <summary>
        /// Установка функции активации.
        /// </summary>
        /// <param name="_Function">Функция активации</param>
        public void SetFunction(IActivationFunction _Function)
        {
            Function = _Function;
        }

        /// <summary>
        /// Получение функции активации.
        /// </summary>
        /// <returns>Функция активации</returns>
        public IActivationFunction GetFunction()
        {
            return Function;
        }

        /// <summary>
        /// Подсчет результата одной из функций активации.
        /// </summary>
        /// <param name="_Value">Входные значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            return GetFunction().Calculate(_Value);
        }
    }

    /// <summary>
    /// Паттерн "Стратегия" для функции активации.
    /// </summary>
    public interface IActivationFunction
    {
        /// <summary>
        /// Вычисление результатов функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns></returns>
        float Calculate(float _Value);
    }

    /// <summary>
    /// Класс "Линейная функция".
    /// </summary>
    public class LinearFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            return _Value;
        }
    }

    /// <summary>
    /// Класс "Гиперболический тангенс".
    /// </summary>
    public class HyperbolicTangentFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            return (float)Math.Tanh(_Value);
        }
    }

    /// <summary>
    /// Класс "Сигмоидная тангенс".
    /// </summary>
    public class SigmoidFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            return (float)(1 / (1 + Math.Exp(-_Value)));
        }
    }

    /// <summary>
    /// Класс "ReLU функция".
    /// </summary>
    public class ReLUFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            return Math.Max(0, _Value);
        }
    }

    /// <summary>
    /// Класс "Пороговая функция".
    /// </summary>
    public class ThresholdFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            if (_Value < 0)
                return 0f;
            else
                return 1f;
        }
    }

    /// <summary>
    /// Класс "Знаковая функция".
    /// </summary>
    public class IconicFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            if (_Value <= 0)
                return -1f;
            else
                return 1f;
        }
    }

    /// <summary>
    /// Класс "Радиальная базисная функция".
    /// </summary>
    public class RadialBaseFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            return (float)Math.Exp(Math.Sqrt(-_Value));
        }
    }

    /// <summary>
    /// Класс "Полулинейная с насыщением функция".
    /// </summary>
    public class SemilinearSaturationFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            if (_Value <= 0)
                return 0f;
            else if (_Value >= 1)
                return 1f;
            else
                return _Value;
        }
    }

    /// <summary>
    /// Класс "Линейная с насыщением функция".
    /// </summary>
    public class LinearSaturationFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            if (_Value <= -1)
                return -1f;
            else if (_Value >= 1)
                return 1f;
            else
                return _Value;
        }
    }

    /// <summary>
    /// Класс "Треугольная функция".
    /// </summary>
    public class TriangularFunction : IActivationFunction
    {
        /// <summary>
        /// Подсчет результата работы функции.
        /// </summary>
        /// <param name="_Value">Входное значение</param>
        /// <returns>Нормализованные данные</returns>
        public float Calculate(float _Value)
        {
            if (Math.Abs(_Value) <= 1)
                return 1 - Math.Abs(_Value);
            else if (Math.Abs(_Value) > 1)
                return 0f;
            else
                return 0f;
        }
    }

    #endregion

    #region TrainingSystem

    /// <summary>
    /// Класс "Система обучения".
    /// </summary>
    public class TrainingSystem
    {
        private ITrainingSystem TrainingSys;

        /// <summary>
        /// Пустой конструктор класса.
        /// </summary>
        public TrainingSystem() { }

        /// <summary>
        /// Конструктор класса с параметрами.
        /// </summary>
        /// <param name="TrainingSys"></param>
        public TrainingSystem(ITrainingSystem TrainingSys)
        {
            SetTraining(TrainingSys);
        }

        /// <summary>
        /// Деструктор класса.
        /// </summary>
        ~TrainingSystem() { }

        /// <summary>
        /// Установка системы обучения.
        /// </summary>
        /// <param name="TrainingSys">Система обучения</param>
        public void SetTraining(ITrainingSystem TrainingSys)
        {
            this.TrainingSys = TrainingSys;
        }

        /// <summary>
        /// Получение системы обучения.
        /// </summary>
        /// <returns>Система обучения</returns>
        public ITrainingSystem GetTraining()
        {
            return TrainingSys;
        }

        /// <summary>
        /// Обучение многослойного перцептрона без учителя.
        /// </summary>
        /// <param name="_ArtificiaNeuralNetwork">Входная нейронная сеть</param>
        /// <param name="_ProbabilityChange">Шанс изменения</param>
        /// <returns>Обученая нейронная сеть</returns>
        public MultilayerPerceptron Training(MultilayerPerceptron _ArtificiaNeuralNetwork, float _ProbabilityChange)
        {
            return GetTraining().Training(_ArtificiaNeuralNetwork, _ProbabilityChange);
        }
    }

    /// <summary>
    /// Паттерн "Стратегия" для системы обучения.
    /// </summary>
    public interface ITrainingSystem
    {
        /// <summary>
        /// Обучение многослойного перцептрона без учителя.
        /// </summary>
        /// <param name="_ArtificiaNeuralNetwork"></param>
        /// <param name="_ProbabilityChange"></param>
        /// <returns></returns>
        MultilayerPerceptron Training(MultilayerPerceptron _ArtificiaNeuralNetwork, float _ProbabilityChange);

        /// <summary>
        /// Получение коофицента для изменения весов связей. 
        /// </summary>
        /// <param name="_ProbabilityChange">Шанс изменения</param>
        /// <returns>Коофицент</returns>
        float ChangeCoefficient(float _ProbabilityChange);
    }

    /// <summary>
    /// Класс "Обучение ИНС без учителя"
    /// </summary>
    public class TeacherlessLearning : ITrainingSystem
    {
        /// <summary>
        /// Обучение многослойного перцептрона без учителя.
        /// </summary>
        /// <param name="_ArtificiaNeuralNetwork">Входная нейронная сеть</param>
        /// <param name="_ProbabilityChange">Шанс изменения</param>
        /// <returns>Обученая нейронная сеть</returns>
        public MultilayerPerceptron Training(MultilayerPerceptron _ArtificiaNeuralNetwork, float _ProbabilityChange)
        {
            List<Layer> new_layers = new List<Layer>();
            List<Synapse> new_synapses = new List<Synapse>();

            for (int i = 0; i < _ArtificiaNeuralNetwork.Layers.Count; i++)
            {
                List<Neuron> new_neurons = new List<Neuron>();
                for (int j = 0; j < _ArtificiaNeuralNetwork.Layers[i].Neurons.Count; j++)
                    new_neurons.Add(new Neuron(_ArtificiaNeuralNetwork.Layers[i].Neurons[j].Value));
                new_layers.Add(new Layer(new_neurons));

                for (int j = 0; j < _ArtificiaNeuralNetwork.Layers[i].Neurons.Count; j++)
                    if (i > 0)
                        for (int k = 0; k < _ArtificiaNeuralNetwork.Layers[i - 1].Neurons.Count; k++)
                            new_synapses.Add(new Synapse(
                                _ArtificiaNeuralNetwork.Layers[i - 1].Neurons[k],
                                _ArtificiaNeuralNetwork.Layers[i].Neurons[j],
                                _ArtificiaNeuralNetwork.Synapses[i + j + k].Value * ChangeCoefficient(_ProbabilityChange)));
            }

            return new MultilayerPerceptron(new_layers, new_synapses);
        }

        /// <summary>
        /// Получение коофицента для изменения весов связей. 
        /// </summary>
        /// <param name="_ProbabilityChange">Шанс изменения</param>
        /// <returns>Коофицент</returns>
        public float ChangeCoefficient(float _ProbabilityChange)
        {
            float RandomСoefficient = UnityEngine.Random.Range(0f, 1f);

            if (RandomСoefficient < _ProbabilityChange)
                return UnityEngine.Random.Range(-1.5f, 1.5f) * UnityEngine.Random.Range(-1.5f, 1.5f);
            else
                return 1f;
        }
    }

    #endregion
}
