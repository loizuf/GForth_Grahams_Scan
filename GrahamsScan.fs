: whichSideLocals { a b c d e f }
	a b c d e f e a - d b - * f b - c a - * - ;

: graham (POINTS LENGTH - CONVEXHULL POINTS)
	1 u+do
		


: whichSide ( x1 y1 x2 y2 x y -- x1 y1 x2 y2 x y d )
	swap >r >r 2swap 2dup r> swap - r> rot - ;

: setup 

create x
create y
