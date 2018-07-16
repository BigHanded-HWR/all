# all
hwr.dll使用方法：
  1.将hwr.dll和模型文件my.pb放在项目的/lib文件夹下（其实只要保证模型文件和dll在同目录下即可）
  2.在项目添加引用（头文件）:using hwr;
  3.在调用前使用InferImage()初始化一个对象，如：var a = new InferImage();
  4.调用这个对象的RecImg(string Nmae)函数，输入一个本地图片路径，图片大小无所谓，内部会进行缩放。
  5.将返回一个string类型的数字，表示对这张图片的估计。
