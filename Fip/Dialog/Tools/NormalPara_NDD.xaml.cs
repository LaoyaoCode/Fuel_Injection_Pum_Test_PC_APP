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
using Fip.MControls;

namespace Fip.Dialog.Tools
{
    /// <summary>
    /// NormalPara_NDD.xaml 的交互逻辑
    /// </summary>
    public partial class NormalPara_NDD : UserControl
    {


        public String ParaName
        {
            get { return (String)GetValue(ParaNameProperty); }
            set { SetValue(ParaNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParaName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParaNameProperty =
            DependencyProperty.Register("ParaName", typeof(String), typeof(NormalPara_NDD), new PropertyMetadata(""));

        //四个组合规则先获取其引用，保持不被删除
        private Grid S_InjectionTime_G = null;
        private Grid R_InjectionQuantity_G = null;
        private Grid R_RackTravel_G = null;
        private Grid R_Asymmetry_G = null;

        public NormalPara_NDD()
        {
            InitializeComponent();
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            S_InjectionTime_G = S_InjectionTimeContainer;
            R_InjectionQuantity_G = R_InjectionQuantityContainer;
            R_RackTravel_G = R_RackTravelContainer;
            R_Asymmetry_G = R_AsymmetryContainer;

            //移除组合规则
            TotalParasContainer.Children.RemoveRange(2, 4);
        }

        /// <summary>
        /// 自由组合规则删除按钮
        /// </summary>
        /// <param name="which"></param>
        private void CloseButton_Click(IconButton which)
        {
            String tag = ((Grid)which.Parent).Tag.ToString();

            Grid rule = null;

            //可视化规则删除
            switch (tag)
            {
                //喷油次数
                case "S_InjectionTime":
                    rule = S_InjectionTime_G;
                    ((TextBox)rule.Children[2]).Text = "";
                    break;
                //喷油量
                case "R_InjectionQuantity":
                    rule = R_InjectionQuantity_G;
                    ((RangeValueInput_NDD)rule.Children[2]).SetRangeValue("", "");
                    break;
                //齿杆行程
                case "R_RackTravel":
                    rule = R_RackTravel_G;
                    ((RangeValueInput_NDD)rule.Children[2]).SetRangeValue("", "");
                    break;
                case "R_Asymmetry":
                    rule = R_Asymmetry_G;
                    ((TextBox)rule.Children[2]).Text = "";
                    break;
            }

            //隐藏规则
            rule.Visibility = Visibility.Collapsed;
            //移除规则
            TotalParasContainer.Children.Remove(rule);


            //给待添加组合规则管理器添加
            for(int counter = 0; counter < RuleSP.Children.Count; counter++)
            {
                //规则标签相同
                if(((Grid)RuleSP.Children[counter]).Tag.ToString() == tag)
                {
                    ((Grid)RuleSP.Children[counter]).Visibility = Visibility.Visible;
                }
            }

            //添加新的组合规则按钮可视化
            AddNewRuleButton.Visibility = Visibility.Visible;
        }

        private void Para_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid container = (Grid)sender;
            container.Children[0].Visibility = Visibility.Visible;
        }

        private void Para_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid container = (Grid)sender;
            container.Children[0].Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 增加新规则按钮被点击
        /// </summary>
        private void AddNewRuleButton_Click()
        {
            NewRuleContainer.IsOpen = false;
            NewRuleContainer.IsOpen = true;
        }

        private void ARuleContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Background = (SolidColorBrush)FindResource("SelectedColor");
        }

        private void ARuleContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Background = (SolidColorBrush)FindResource("AppBackBrush");
        }

        private void ARuleContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //获取标签
            String tag = ((Grid)sender).Tag.ToString();

            //从待添加规则列表里面移除(实际上只是隐藏了而已)
            ((Grid)sender).Visibility = Visibility.Collapsed;

            Grid rule = null;

            //可视化规则添加
            switch (tag)
            {
                //喷油次数
                case "S_InjectionTime":
                    rule = S_InjectionTime_G;
                    break;
                //喷油量
                case "R_InjectionQuantity":
                    rule = R_InjectionQuantity_G;
                    break;
                //齿杆行程
                case "R_RackTravel":
                    rule = R_RackTravel_G;
                    break;
                case "R_Asymmetry":
                    rule = R_Asymmetry_G;
                    break;
            }

            //添加到容器中
            rule.Visibility = Visibility.Visible;
            TotalParasContainer.Children.Add(rule);

            //关闭popup，
            NewRuleContainer.IsOpen = false;


            int counter = 0;
            for(counter = 0; counter < RuleSP.Children.Count; counter++)
            {
                if(((Grid)RuleSP.Children[counter]).Visibility == Visibility.Visible)
                {
                    break;
                }
            }
            //没有剩余的规则了，隐藏添加按钮
            if (counter == RuleSP.Children.Count)
            {
                AddNewRuleButton.Visibility = Visibility.Collapsed;
                
            }
        }

       /// <summary>
       /// 检查值是否合理
       /// </summary>
       /// <returns></returns>
        public bool IsValueRight()
        {
            bool result = true;
            String speedString = S_RotateSpeed_TB.Text;
            int speed;

            //速度无法解析为整数 , 或者输入数据为负数
            if (!int.TryParse(speedString.Trim(), out speed) || speed < 0)
            {
                result = false;
            }

            for (int counter = 2; counter < TotalParasContainer.Children.Count; counter++)
            {
                String tag = ((Grid)TotalParasContainer.Children[counter]).Tag.ToString();

                //为范围数
                if (tag == "R_InjectionQuantity" || tag == "R_RackTravel")
                {
                    RangeValueInput_NDD value = (RangeValueInput_NDD)(((Grid)TotalParasContainer.Children[counter]).Children[2]);
                    //范围数据出现了错误
                    if (!value.IsValueRight())
                    {
                        result = false;
                        break;
                    }
                }
                //喷油次数，单数，整数
                else if(tag == "S_InjectionTime")
                {
                    int value; 
                    if (!int.TryParse(((TextBox)(((Grid)TotalParasContainer.Children[counter]).Children[2])).Text.Trim(), out value) || value < 0)
                    {
                        result = false;
                        break;
                    }
                }
                //不均匀度，单数，浮点
                else
                {
                    float value;
                    if (!float.TryParse(((TextBox)(((Grid)TotalParasContainer.Children[counter]).Children[2])).Text.Trim(), out value) || value < 0)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
