using System;
using System.Windows;
using WindowsInput;
using System.Windows.Input;
using Shipwreck.Phash;
using System.Drawing;
using Shipwreck.Phash.Bitmaps;

namespace HobbitAutoSplitter
{
    public class LivesplitManager
    {
        private bool started = false;
        private bool paused = false;
        private bool split = false;
        private bool waitingToStart = false;
        private bool thiefTextBoxDone = false;
        private bool thiefSplit = false;
        private int level = 0;
        private int detectionLevel = 0;

        InputSimulator sim = new InputSimulator();

        public void HandleTextOutput(string output)
        {
            string comparison = output.ToLower().Replace(" ", string.Empty);
            if (!string.IsNullOrEmpty(comparison))
            {
                if (comparison.Contains(Constants.loadingKeyword))
                {
                    if(detectionLevel < 11)
                    {
                        if (comparison.Contains(Constants.levelsDetection[detectionLevel]) && !split)
                        {
                            detectionLevel++;
                            level++;
                            split = true;
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                MainWindow.instance.SetLevelText(level);
                                Keyboard.ClearFocus();                                
                            }));
                            sim.Keyboard.KeyDown(Settings.Default.split);
                            if (paused)
                            {
                                sim.Keyboard.KeyDown(Settings.Default.split);
                                sim.Keyboard.KeyDown(Settings.Default.pause);
                            }
                        }
                    }
                    if (!paused)
                    {                        
                        paused = true;
                        sim.Keyboard.KeyDown(Settings.Default.pause);
                    }        
                }
                else
                {
                    if (paused)
                    {
                        if (detectionLevel == 7 && comparison.Contains("web"))
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                Keyboard.ClearFocus();
                                MainWindow.instance.SetStatus(States.GAMEPLAY);
                            }));
                            paused = false;
                            sim.Keyboard.KeyDown(Settings.Default.pause);
                        }
                        if (comparison.Contains("press"))
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                Keyboard.ClearFocus();
                                MainWindow.instance.SetStatus(States.GAMEPLAY);
                            }));
                            paused = false;
                            sim.Keyboard.KeyDown(Settings.Default.pause);
                        }
                    }
                    else
                    {
                        if (detectionLevel == 8 && comparison.Contains("away!"))
                        {
                            if (thiefTextBoxDone)
                            {
                                if (!thiefSplit)
                                {
                                    sim.Keyboard.KeyDown(Settings.Default.split);
                                }
                                else
                                {
                                    sim.Keyboard.KeyDown(Settings.Default.unsplit);
                                    sim.Keyboard.KeyDown(Settings.Default.split);
                                }

                                thiefSplit = true;
                                thiefTextBoxDone = false;
                            }
                        } 
                    }
                    foreach (string resetKeyword in Constants.resetKeywords)
                    {
                        if (comparison.Contains(resetKeyword))
                        {
                            if (started)
                            {
                                started = false;
                                waitingToStart = true;
                                thiefTextBoxDone = false;
                                thiefSplit = false;
                                level = 0;
                                detectionLevel = 0;
                                Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    MainWindow.instance.SetLevelText(level);
                                    MainWindow.instance.SetStatus(States.READYTOSTART);
                                    Keyboard.ClearFocus();
                                }));
                                sim.Keyboard.KeyDown(Settings.Default.reset);
                            }
                            else
                            {
                                started = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (paused)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Keyboard.ClearFocus();
                        MainWindow.instance.SetStatus(States.GAMEPLAY);
                    }));
                    split = false;
                    paused = false;
                    sim.Keyboard.KeyDown(Settings.Default.pause);
                }

                if (!thiefTextBoxDone)
                {
                    thiefTextBoxDone = true;
                }

                if (waitingToStart)
                {
                    started = true;
                    waitingToStart = false;
                    level++;
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        MainWindow.instance.SetStatus(States.STARTED);
                        MainWindow.instance.SetLevelText(level);
                        Keyboard.ClearFocus();
                    }));
                    sim.Keyboard.KeyDown(Settings.Default.split);
                }
            }
        }

        public void DetectFinalFrame(Bitmap current, Bitmap reference)
        {
            if(reference != null)
            {
                Digest currentDigest = ImagePhash.ComputeDigest(ImageProcessor.EndFrameCrop(current).ToLuminanceImage());
                Digest referenceDigest = ImagePhash.ComputeDigest(ImageProcessor.EndFrameCrop(reference).ToLuminanceImage());

                float score = ImagePhash.GetCrossCorrelation(referenceDigest, currentDigest);
                if (score > .9f)
                {
                    sim.Keyboard.KeyDown(Settings.Default.split);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        MainWindow.instance.SetStatus(States.FINISHED);
                    }));
                }
            }
        }

        public bool onTCB()
        {
            return detectionLevel == 11;
        }
    }
}
