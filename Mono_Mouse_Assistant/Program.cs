using System;
using System.Diagnostics;
using System.Windows.Forms;
using Cairo;
using Gtk;

public class GtkMouseAssistant
{
    enum Phase
    {
        WAIT_FOR_MOVE,
        IDLE_AFTER_MOVE,
        ANIMATION,
        CLICK_ANIMATION,

    }

    private DrawingArea drawingArea;
    private Phase phase = Phase.WAIT_FOR_MOVE;
    private int WinMaxSize = 105;
    private int WinMinSize = 60;
    private float ProgressFactor = 1;
    private int IdleTime = 300;
    private int ClickAnimationTime = 200;
    private int WaitBeforeClick = 1000;
    private System.Drawing.Point LastMousePos = new System.Drawing.Point();
    private Stopwatch AnimationTimer = new Stopwatch();
    private Stopwatch IdleTimer = new Stopwatch();
    private Stopwatch ClickAnimationTimer = new Stopwatch();
    private Gtk.Window Window;

    public static void Main()
    {
        new GtkMouseAssistant();
    }






    public GtkMouseAssistant()
    {
        Console.WriteLine("Start");
        Gtk.Application.Init();
        var window = new Gtk.Window("Mouse Assistant");
        // Turn off title bar
        window.Decorated = false;

        // Create Cairo drawing area
        drawingArea = new DrawingArea();
        drawingArea.ScreenChanged += OnScreenChanged;
        drawingArea.ExposeEvent += OnExposeEvent;

        GLib.Timeout.Add(20, new GLib.TimeoutHandler(update_status));

        // Add drawing area to window
        var box = new HBox(true, 0);

        box.Add(drawingArea);

        window.Add(box);
        window.ShowAll();

        window.KeepAbove = true;
        window.CanFocus = false;
        window.Resize(this.WinMaxSize, this.WinMaxSize);

        Window = window;
       //  create_input_mask();
        Gtk.Application.Run();
    }

    // needed for click through
    void create_input_mask()
    {

        Gdk.Pixmap pixmap = new Gdk.Pixmap(Window.RootWindow, WinMaxSize, WinMaxSize, 1);
        Window.InputShapeCombineMask(pixmap, 0, 0);
    }


    bool update_status()
    {
        // move to cursor pos
        Window.Move(Cursor.Position.X - WinMaxSize / 2, Cursor.Position.Y - WinMaxSize / 2);


        var pos = Cursor.Position;

        if (!pos.Equals(LastMousePos) && phase != Phase.CLICK_ANIMATION)
        {
            Console.WriteLine("POS CHANGED");
            IdleTimer.Restart();
            ProgressFactor = 0;
            LastMousePos = pos;

            phase = Phase.IDLE_AFTER_MOVE;
        }
        else
        {
            if (phase == Phase.CLICK_ANIMATION && ClickAnimationTimer.ElapsedMilliseconds > ClickAnimationTime)
            {
                Console.WriteLine("Finish click animation");
                phase = Phase.WAIT_FOR_MOVE;
            }

            if (phase == Phase.IDLE_AFTER_MOVE)
            {
                if (IdleTimer.ElapsedMilliseconds > IdleTime)
                {
                    Console.WriteLine("Start aninmation");
                    AnimationTimer.Restart();
                    phase = Phase.ANIMATION;
                }
            }
            else if (phase == Phase.ANIMATION)
            {
                //                idle phase
                long timeElapsed = (long)(AnimationTimer.Elapsed.TotalMilliseconds);
                if (timeElapsed == 0) { return true; }


                ProgressFactor = (float)Math.Min(timeElapsed, WaitBeforeClick) / (float)WaitBeforeClick;
                // Update the animation
                Console.WriteLine("Animation p " + ProgressFactor);
                if (timeElapsed > WaitBeforeClick)
                {
                    // click
                    Console.WriteLine("Click");
                    MouseClicker.click(Cursor.Position.X, Cursor.Position.Y);
                    ProgressFactor = 0;
                    phase = Phase.CLICK_ANIMATION;
                    ClickAnimationTimer.Restart();
                }

            }
        }
        drawingArea.QueueDraw();

        return true;
    }

    protected void OnScreenChanged(object o, ScreenChangedArgs args)
    {
        // Use the RGBA colour map, so that alpha values work
        drawingArea.Screen.DefaultColormap = drawingArea.Screen.RgbaColormap;
    }

    protected void OnExposeEvent(object o, ExposeEventArgs args)
    {

        /*using (Context ctx = Gdk.CairoHelper.Create(drawingArea.GdkWindow))
        {
            Console.WriteLine("Draw start");
            // Paint a semitransparent colour onto the background
            ctx.SetSourceRGBA(0, 0, 0, 0.0);
            ctx.Operator = Operator.Source;
            ctx.Paint();
            DrawingArea area = (DrawingArea)o;
            Cairo.Context cr = Gdk.CairoHelper.Create(area.GdkWindow);
            if (phase == Phase.CLICK_ANIMATION)
            {
                float progress_factor = Math.Min(ClickAnimationTimer.ElapsedMilliseconds, ClickAnimationTime) / (float)ClickAnimationTime;

                cr.LineWidth = 3;
                cr.SetSourceRGBA(1, 0.64, 0, 1 - progress_factor);

                cr.Translate(WinMaxSize / 2, WinMaxSize / 2);


                float radius = WinMinSize + (WinMaxSize - WinMinSize) * progress_factor;
                cr.Arc(0, 0, radius / 2, 0, 2 * Math.PI);
                cr.StrokePreserve();

                cr.SetSourceRGBA(0, 0, 0, 0);
                cr.Fill();

            }
            else if (phase == Phase.ANIMATION)
            {

                cr.LineWidth = 3;
                cr.SetSourceRGB(1, 0.64, 0);
                cr.Translate(WinMaxSize / 2, WinMaxSize / 2);
                cr.LineTo(new PointD(0, 0));
                double angle_offset = 90.0 * (Math.PI / 180.0);  
                cr.Arc(0, 0, WinMinSize / 2, -angle_offset, ProgressFactor * 2 * Math.PI - angle_offset);
                cr.LineTo(new PointD(0, 0));

                cr.StrokePreserve();


                cr.SetSourceRGBA(0, 0, 0, 0);
                cr.Fill();


            }
            ((IDisposable)cr.Target).Dispose();
            ((IDisposable)cr).Dispose();
            cr.SetT
            Console.WriteLine("Draw finish");
        }
    }*/
        using (Context cr = Gdk.CairoHelper.Create(drawingArea.GdkWindow))
        {
            cr.MoveTo(0, 0);
            cr.LineTo(500, 500);
            cr.Color = new Color(1, 0, 1);
            cr.Stroke();
        }
    }
    }