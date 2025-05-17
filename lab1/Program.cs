using lab1;

Point2d.WIDTH = 800;
Point2d.HEIGHT = 800;


//точки
Point2d pnt1 = new Point2d(1, 1);
Point2d pnt2 = new Point2d(200, 10);

System.Console.WriteLine(pnt1.Equals(pnt2));


//векторы
Vector2d vct1 = new Vector2d(1, 11);
Vector2d vct2 = new Vector2d(pnt1, pnt2);

System.Console.WriteLine($"{vct1[0]} {vct1[1]}");
System.Console.WriteLine(vct2.ToString());

System.Console.WriteLine(vct2.Length());

System.Console.WriteLine(vct1.Equals(vct2));

System.Console.WriteLine(vct1 - vct2);

System.Console.WriteLine(vct1 * 5);

System.Console.WriteLine((vct2 / 20).ToString());

System.Console.WriteLine(Vector2d.ScalarMult(vct1, vct2));

// только через $"...", больше памяти экономит
// sourcery extension для рефакторинга

// ctrl shift i