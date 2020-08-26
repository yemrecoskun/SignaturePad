using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Graphics;

namespace SignaturePad
{
    class SignaturePolyline
    {
        public SignaturePolyline()
        {
            Path = new Path();
        }
        public Color Color { set; get; }
        public float StrokeWidth { set; get; }
        public Path Path { private set; get; }
    }
    public class Coordinate
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Pressure { get; set; }
        public DateTime StrokeTime { get; set; }
        public byte SignID { get; set; }
    }
    public class SignaturePadCanvasView : View
    {
        // Two collections for storing polylines
        Dictionary<int, SignaturePolyline> inProgressPolylines = new Dictionary<int, SignaturePolyline>();
        List<SignaturePolyline> completedPolylines = new List<SignaturePolyline>();
        byte SignIDx = 0;
        public List<Coordinate> Coordinates { get; set; } = new List<Coordinate>();
        Paint paint = new Paint();
        public SignaturePadCanvasView(Context context) : base(context)
        {
            Initialize();
        }
        public SignaturePadCanvasView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }
        void Initialize()
        {

        }
        public Color StrokeColor { set; get; } = Color.Black;
        public float StrokeWidth { set; get; } = 10;
        public void ClearAll()
        {
            Coordinates.Clear();
            completedPolylines.Clear();
            Invalidate();
        }
        public override bool OnTouchEvent(MotionEvent args)
        {
            int pointerIndex = args.ActionIndex;
            Coordinates.Add(new Coordinate
            {
                X = args.GetX(pointerIndex),
                Y = args.GetY(pointerIndex),
                Pressure = args.GetPressure(pointerIndex),
                StrokeTime = DateTime.Now,
                SignID = SignIDx
            });
            int id = args.GetPointerId(pointerIndex);
            switch (args.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    SignaturePolyline polyline = new SignaturePolyline
                    {
                        Color = StrokeColor,
                        StrokeWidth = StrokeWidth
                    };
                    polyline.Path.MoveTo(args.GetX(pointerIndex),
                                         args.GetY(pointerIndex));
                    inProgressPolylines.Add(id, polyline);
                    break;
                case MotionEventActions.Move:
                    for (pointerIndex = 0; pointerIndex < args.PointerCount; pointerIndex++)
                    {
                        id = args.GetPointerId(pointerIndex);
                        inProgressPolylines[id].Path.LineTo(args.GetX(pointerIndex),
                                                            args.GetY(pointerIndex));
                    }
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                    SignIDx++;
                    inProgressPolylines[id].Path.LineTo(args.GetX(pointerIndex),
                                                        args.GetY(pointerIndex));
                    completedPolylines.Add(inProgressPolylines[id]);
                    inProgressPolylines.Remove(id);
                    break;
                case MotionEventActions.Cancel:
                    inProgressPolylines.Remove(id);
                    break;
            }
            Invalidate();
            return true;
        }
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            paint.SetStyle(Paint.Style.Fill);
            paint.Color = Color.White;
            canvas.DrawPaint(paint);
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeCap = Paint.Cap.Round;
            paint.StrokeJoin = Paint.Join.Round;
            foreach (SignaturePolyline polyline in completedPolylines)
            {
                paint.Color = polyline.Color;
                paint.StrokeWidth = polyline.StrokeWidth;
                canvas.DrawPath(polyline.Path, paint);
            }
            foreach (SignaturePolyline polyline in inProgressPolylines.Values)
            {
                paint.Color = polyline.Color;
                paint.StrokeWidth = polyline.StrokeWidth;
                canvas.DrawPath(polyline.Path, paint);
            }
        }
    }
}