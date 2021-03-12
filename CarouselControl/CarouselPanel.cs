using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarouselControl
{
    public class CarouselPanel : Control
    {
        CarouselImageControl CarouselImage = null;
        public CarouselPanel()
        {
            CarouselImage = new CarouselImageControl();
            Controls.Add(CarouselImage);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            CarouselImage.Size = Size;
        }

        public IDictionary<Image, Action> ImageAndActions
        {
            set => CarouselImage.ImageAndActions = value;
        }

        private class CarouselImageControl : Control
        {
            private IDictionary<Image, Action> _ImageAndActions;
            private Timer tAutoCorrection = null;
            private Timer tAutoPlay = new Timer();
            private HashSet<int> lstPause = new HashSet<int>();

            public IDictionary<Image, Action> ImageAndActions
            {
                set
                {
                    _ImageAndActions = value;
                    int iLeftOrder = 0;
                    foreach (Image img in _ImageAndActions.Keys)
                    {
                        Panel pnlImage = new Panel();
                        pnlImage.Size = new Size(Width, Height);
                        pnlImage.Location = new Point(iLeftOrder * Width, 0);
                        pnlImage.BackgroundImage = img;
                        pnlImage.BackgroundImageLayout = ImageLayout.Zoom;
                        pnlImage.Margin = new Padding(0);

                        bool isMove = false;
                        pnlImage.MouseClick += (_, ClickE) =>
                        {
                            if (!isMove)
                                _ImageAndActions[img]?.Invoke();
                        };
                        int LastMouseMoveHorPoint = 0;
                        pnlImage.MouseDown += (_, DownE) =>
                        {
                            if (DownE.Button == MouseButtons.Left)
                                LastMouseMoveHorPoint = DownE.X;
                        };
                        pnlImage.MouseMove += (_, MoveE) =>
                        {
                            isMove = false;
                            if (MoveE.Button == MouseButtons.Left)
                            {
                                tAutoPlay.Stop();
                                if (tAutoCorrection != null)
                                {
                                    tAutoCorrection.Stop();
                                    tAutoCorrection.Dispose();
                                }
                                isMove = true;
                                if (MoveE.X < LastMouseMoveHorPoint)
                                    Left -= LastMouseMoveHorPoint - MoveE.X;
                                if (MoveE.X > LastMouseMoveHorPoint)
                                    Left += MoveE.X - LastMouseMoveHorPoint;
                            }
                        };
                        pnlImage.MouseUp += (_, UpE) =>
                        {
                            int negativeLeft = Left * -1;
                            if (!lstPause.Contains(negativeLeft))
                            {
                                if (!lstPause.Contains(negativeLeft))
                                {
                                    if (tAutoCorrection != null)
                                    {
                                        tAutoCorrection.Stop();
                                        tAutoCorrection.Dispose();
                                    }
                                    tAutoCorrection = new Timer();
                                    tAutoCorrection.Interval = 10;
                                    int closedItemLeft = Controls.OfType<Panel>().Select(p => p.Left).ToArray().Binarysearch(negativeLeft) * -1;
                                    tAutoCorrection.Tick += (sender, e) =>
                                    {
                                        if (Left > closedItemLeft)
                                        {
                                            int leftSpace = Math.Abs( closedItemLeft - Left );
                                            if (leftSpace == 0)
                                            {
                                                tAutoCorrection.Stop();
                                                tAutoPlay.Start();
                                            }
                                            else if (leftSpace > 0 && leftSpace <= 30)
                                                Left -= 1;
                                            else
                                                Left -= 30;
                                        }
                                        else
                                        {
                                            int leftSpace = Math.Abs(closedItemLeft - Left);
                                            if (leftSpace == 0)
                                            {
                                                tAutoCorrection.Stop();
                                                tAutoPlay.Start();
                                            }
                                            else if (leftSpace <= 30)
                                                Left += 1;
                                            else
                                                Left += 30;
                                        }
                                    };
                                    tAutoCorrection.Start();
                                }
                            }
                        };
                        Controls.Add(pnlImage);
                        iLeftOrder++;
                    }

                    bool getBack = false;
                    tAutoPlay.Interval = 3000;
                    tAutoPlay.Tick += (_, PlayE) =>
                    {
                        tAutoPlay.Interval = 10;
                        int absLeft = Math.Abs(Left);
                        if (absLeft < Width - Parent.Width && !getBack)
                        {
                            Left -= 30;
                            absLeft = Math.Abs(Left);
                            if (lstPause.Contains(absLeft))
                                tAutoPlay.Interval = 3000;
                        }
                        else
                        {
                            getBack = true;
                            Left += 100;
                            if (Left == 0)
                            {
                                getBack = false;
                                tAutoPlay.Interval = 3000;
                            }
                        }
                    };
                    Size = new Size(Controls.OfType<Panel>().Sum(p => p.Width), Height);

                    int multipleWidth = 0;
                    lstPause.Add(0);
                    while (multipleWidth < Width)
                    {
                        multipleWidth += Parent.Width;
                        lstPause.Add(multipleWidth);
                    }

                    tAutoPlay.Start();
                }
            }
        }
    }
}
