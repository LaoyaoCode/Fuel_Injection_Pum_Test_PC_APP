﻿using System;
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
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Fip.Dialog.Tools;
using Fip.Code.DB;
using Fip.MControls;

namespace Fip.Dialog
{
    /// <summary>
    /// NewDeviceDialog.xaml 的交互逻辑
    /// </summary>
    public partial class NewDeviceDialog : Window
    {
        /// <summary>
        /// 现在正在活跃的界面
        /// </summary>
        private CircleButton_NDD NowActiveButton = null;
        /// <summary>
        /// 内容的宽度
        /// </summary>
        private const double CONTENT_WIDTH = 580;

        private StandardDeviceDesModel _Result = null;

        private StandardDeviceDesModel AdvanceData = null;
        private bool IsModify = false;

        public NewDeviceDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 预先填装数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isModify">是否是修改数据</param>
        public NewDeviceDialog(StandardDeviceDesModel model , bool isModify = false)
        {
            InitializeComponent();
            AdvanceData = model;
            IsModify = isModify;
        }

        /// <summary>
        /// 窗口top bar拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DargWindowMove(object sender, MouseButtonEventArgs e)
        {
            //move the windows
            this.DragMove();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void CloseButton_Click()
        {
            this.DialogResult = false;
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(CircleButton_NDD button in StepButtonContainer.Children)
            {
                button.ClickEvent = StepButton_Click;
            }

            ((CircleButton_NDD)StepButtonContainer.Children[0]).VirtualButton_Click();

            if(AdvanceData != null)
            {
                StartWork_NP.SetMessage(AdvanceData.StartWork);
                IdlingWork_NP.SetMessage(AdvanceData.IdlingWork);
                IdlingBreak_NP.SetMessage(AdvanceData.IdlingBreak);
                ReviseBegin_NP.SetMessage(AdvanceData.ReviseBegin);
                ReviseWork_NP.SetMessage(AdvanceData.ReviseWork);
                ReviseEnd_NP.SetMessage(AdvanceData.ReviseEnd);
                DemWork_NP.SetMessage(AdvanceData.DemWork);
                AdjWork_NP.SetMessage(AdvanceData.AdjWork);
                HighBreak_NP.SetMessage(AdvanceData.HighBreak);

                SpecialPara.SetMessage(AdvanceData.EquCode, AdvanceData.EquType, AdvanceData.Tem, IsModify);
            }

            
        }

        private void StepButton_Click(String tag , CircleButton_NDD which)
        {
            if (NowActiveButton != null)
            {
                //检查数据，然后根据数据来判断修改状态枚举
                int index = int.Parse(NowActiveButton.ButtonTag);

                if(index <= 8)
                {
                    NormalPara_NDD value = (NormalPara_NDD)((Grid)(MainContent.Children[index])).Children[0];

                    if(value.IsValueRight())
                    {
                        NowActiveButton.ChangeState(CircleButton_NDD.StateEnum.Finish);
                    }
                    else
                    {
                        NowActiveButton.ChangeState(CircleButton_NDD.StateEnum.Error);
                    }
                }
                else
                {
                    SpecialPara_NDD value = (SpecialPara_NDD)((Grid)(MainContent.Children[index])).Children[0];
                    if (value.IsValueRight())
                    {
                        NowActiveButton.ChangeState(CircleButton_NDD.StateEnum.Finish);
                    }
                    else
                    {
                        NowActiveButton.ChangeState(CircleButton_NDD.StateEnum.Error);
                    }
                }

                //返回到普通的状态，并且根据之前修改的状态枚举来改变颜色
                NowActiveButton.BackToNormalCondition();
            }

            //根据tag来执行动画
            StepAnimation(int.Parse(tag));
            NowActiveButton = which;
        }

        /// <summary>
        /// 切换场景动画
        /// </summary>
        /// <param name="index"></param>
        private void StepAnimation(int index)
        {
            Thickness now = MainContent.Margin;
            Thickness target = MainContent.Margin;
            target.Left = -CONTENT_WIDTH * index;
            int time = (int)(Math.Sqrt(Math.Abs(target.Left - now.Left))) * 10;
            ThicknessAnimation animation = new ThicknessAnimation(now, target, new Duration(new TimeSpan(0,0,0,0, time) ) );
            MainContent.BeginAnimation(Grid.MarginProperty, animation);
        }

        private void SubmitButtonClick()
        {
            StandardDeviceDesModel sModel = new StandardDeviceDesModel();

            for(int counter = 0; counter  < MainContent.Children.Count; counter++)
            {
                if(counter <= 8)
                {
                    if(!((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).IsValueRight())
                    {
                        MessageDialog dialog = new MessageDialog("数据填写不正确(红色按钮部分)!");
                        if(dialog.ShowDialog().Value)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (!((SpecialPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).IsValueRight())
                    {
                        MessageDialog dialog = new MessageDialog("数据填写不正确(红色按钮部分)!");
                        if (dialog.ShowDialog().Value)
                        {
                            return;
                        }
                    }
                }

                //填装数据
                switch(counter)
                {
                    case 0:
                        sModel.StartWork = ((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).GetModelValue();
                        break;
                    case 1:
                        sModel.IdlingWork = ((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).GetModelValue();
                        break;
                    case 2:
                        sModel.IdlingBreak = ((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).GetModelValue();
                        break;
                    case 3:
                        sModel.ReviseBegin = ((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).GetModelValue();
                        break;
                    case 4:
                        sModel.ReviseWork = ((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).GetModelValue();
                        break;
                    case 5:
                        sModel.ReviseEnd = ((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).GetModelValue();
                        break;
                    case 6:
                        sModel.DemWork = ((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).GetModelValue();
                        break;
                    case 7:
                        sModel.AdjWork = ((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).GetModelValue();
                        break;
                    case 8:
                        sModel.HighBreak = ((NormalPara_NDD)((Grid)MainContent.Children[counter]).Children[0]).GetModelValue();
                        break;
                    case 9:
                        SpecialPara_NDD sPara = (SpecialPara_NDD)((Grid)MainContent.Children[counter]).Children[0];
                        sModel.Tem = sPara.GetTem();
                        sModel.EquCode = sPara.GetEquCode();
                        sModel.EquType = sPara.GetEquType();
                        break;
                }
            }

            _Result = sModel;

            //数据填写正确
            this.DialogResult = true;
        }

        /// <summary>
        /// 获得添加的结果数据,该数据并没有保存到数据库之中
        /// </summary>
        /// <returns></returns>
        public StandardDeviceDesModel GetResult()
        {
            return _Result;
        }
    }
}
