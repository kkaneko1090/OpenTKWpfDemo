using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Wpf;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenTKWpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //GLコントロールの設定
            var settings = new GLWpfControlSettings()
            {
                MajorVersion = 3,
                MinorVersion = 1,
                RenderContinuously = true,
            };

            //描画開始
            glControl.Start(settings);

        }

        #region メンバ変数
        //線分の角度
        float angle = 0;
        //線分の回転半径
        float radius = 200;
        //回転中フラグ
        bool isRotating = false;
        #endregion

        /// <summary>
        /// Readyイベント
        /// </summary>
        private void glControl_Ready()
        {
            //背景を黒色でクリア
            GL.ClearColor(Color4.Black);
            //デプステストを有効化
            GL.Enable(EnableCap.DepthTest);
            //カリングの設定
            GL.Enable(EnableCap.CullFace); //カリングの有効化
            GL.CullFace(CullFaceMode.Back); //裏面をカリングする面に指定
            GL.FrontFace(FrontFaceDirection.Ccw); //反時計回りで描かれる面を表面に指定
            //ライトの設定
            GL.Enable(EnableCap.Lighting); //ライティングの有効化
            GL.Enable(EnableCap.Light0); //ライトを1つ（０番）を有効化
            GL.Light(LightName.Light0, LightParameter.Position, Vector4.UnitZ); //UnitZ(0,0,1)にライトを配置
            //法線の正規化
            GL.Enable(EnableCap.Normalize);
            //物体の質感の設定
            GL.Enable(EnableCap.ColorMaterial); //質感の有効化
            GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.Diffuse);//表面を拡散反射する質感に指定

        }

        /// <summary>
        /// Renderイベント
        /// </summary>
        /// <param name="obj"></param>
        private void glControl_Render(TimeSpan obj)
        {
            //カラーバッファ初期化
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //デプスバッファ初期化
            GL.Clear(ClearBufferMask.DepthBufferBit);

            //描画領域を指定する２点のXY座標を指定
            GL.Viewport(0, 0, (int)glControl.ActualWidth, (int)glControl.ActualHeight); //(0,0)と((int)glControl.ActualWidth, (int)glControl.ActualHeight)が成す長方形領域を指定
            //視点を設定
            Matrix4 modelView = Matrix4.LookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY); //UnitZ(0,0,1)からZero(0,0,0)の向きに見るように指定，画面の上方向はUnitY(0,1,0)と指定
            //マトリックスの読み込みモードをModelView（視点）に変更
            GL.MatrixMode(MatrixMode.Modelview);
            //視点行列を読み込み
            GL.LoadMatrix(ref modelView);

            //投影領域を設定
            Matrix4 projection = Matrix4.CreateOrthographic((float)glControl.ActualWidth, (float)glControl.ActualHeight, -500.0f, 500.0f);
            //マトリクスの読み込みモードをProjection（投影領域）に変更
            GL.MatrixMode(MatrixMode.Projection);
            //投影行列を読み込み
            GL.LoadMatrix(ref projection);


            //線分の描画
            DrawLineSegment();

        }

        /// <summary>
        /// 線分の描画
        /// </summary>
        private void DrawLineSegment()
        {

            #region 描画処理
            //線の幅を設定
            GL.LineWidth(1);
            //Lineモードで描画開始
            GL.Begin(PrimitiveType.Lines);
            //色指定
            GL.Color4(Color4.Red);
            //線分の始点
            GL.Vertex3(0, 0, 0);
            //線分の終点
            GL.Vertex3(radius * MathF.Cos(angle), radius * MathF.Sin(angle), 0);
            //描画終了
            GL.End();

            //実行中フラグのとき
            if (isRotating)
            {
                //角度をインクリメント
                angle += 0.01f;
                //角度を正規化
                angle = angle % (2 * MathF.PI);
                //ラベルに角度を表示
                lblAngle.Content = string.Format("Angle: {0} deg.", (angle / MathF.PI * 180).ToString("F2"));
            }
            #endregion
        }


        /// <summary>
        /// ボタンによる処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            isRotating = !isRotating;
        }

    }
}