using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fip.Dialog.Tools
{
    /// <summary>
    /// RangeValueInput_NDD.xaml 的交互逻辑
    /// </summary>
    public partial class RangeValueInput_NDD : UserControl
    {
        /// <summary>
        /// 最大值
        /// </summary>
        private float _Max = -1;
        /// <summary>
        /// 最小值
        /// </summary>
        private float _Min = -1;
        /// <summary>
        /// 是否到达正无限
        /// </summary>
        private bool IsInfinity = false;

        public RangeValueInput_NDD()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public RangeValueInput_NDD(float min , float max)
        {
            InitializeComponent();

            if(max < min)
            {
                throw new Exception("max must bigger than min");
            }

            _Max = max;
            _Min = min;

            MaxValueTB.Text = _Max.ToString();
            MinValueTB.Text = _Min.ToString();

            //检查用户输入
            CheckTheValue();
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        public float GetMax()
        {
            return _Max;
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns></returns>
        public float GetMin()
        {
            return _Min;
        }
        /// <summary>
        /// 检查输入值，给予用户提示
        /// </summary>
        private void CheckTheValue()
        {
            float min, max;
            String maxS = MaxValueTB.Text.Trim(), minS = MinValueTB.Text.Trim();

            //最大值为无限
            if (maxS == "$")
            {
                _Max = float.MaxValue;
                StateRect_Max.Fill = (SolidColorBrush)FindResource("SpecialColor2");
                IsInfinity = true;
            }
            //成功转换最大值,并且为正数
            else if (float.TryParse(maxS, out max) && max > 0)
            {
                _Max = max;
                StateRect_Max.Fill = (SolidColorBrush)FindResource("SpecialColor2");
                IsInfinity = false;
            }
            //转化错误
            else
            {
                _Max = -1;
                StateRect_Max.Fill = (SolidColorBrush)FindResource("SpecialColor3");
                IsInfinity = false;
            }


            //成功转换最小值，并且为正数或0
            if (float.TryParse(minS, out min) && min >= 0)
            {
                _Min = min;
                StateRect_Min.Fill = (SolidColorBrush)FindResource("SpecialColor2");
            }
            //转化错误
            else
            {
                _Min = -1;
                StateRect_Min.Fill = (SolidColorBrush)FindResource("SpecialColor3");
            }

            if (_Max <= _Min)
            {
                ErrorIcon.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorIcon.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="max">最大值字符串</param>
        /// <param name="min">最小值字符串</param>
        public void SetRangeValue(String max , String min)
        {
            MaxValueTB.Text = max;
            MinValueTB.Text = min;

            CheckTheValue();
        }
        /// <summary>
        /// 输入值是否符合要求
        /// </summary>
        /// <returns></returns>
        public bool IsValueRight()
        {
            if(_Max > 0 && _Min >= 0 && _Max > _Min)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MinValueTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTheValue();
        }

        private void MaxValueTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckTheValue();
        }
    }
}
