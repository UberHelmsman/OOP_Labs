using lab1;

Point2d.WIDTH = 1000;
Point2d.HEIGHT = 800;


//точки
Point2d pnt1 = new Point2d(1,1);
Point2d pnt2 = new Point2d(200,10);

System.Console.WriteLine(pnt1.Equals(pnt2));


//векторы
Vector2d vct1 = new Vector2d(1,11);
Vector2d vct2 = new Vector2d(pnt1,pnt2);

System.Console.WriteLine(vct1[0] + " " + vct1[1]);
System.Console.WriteLine(vct2.ToString());

System.Console.WriteLine(vct2.Length());