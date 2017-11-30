: whichSideLocals { a b c d e f }
	a b c d e f e a - d b - * f b - c a - * - ;

: graham ( POINTS LENGTH - CONVEXHULL POINTS )
	dup 2 * 
	6 u+do
		swap >r
	loop

	3 u+do
		begin
			whichSideLocals .s 0 > while
				drop drop
		repeat
		r> r>
	loop ;
		 
: whichSide ( x1 y1 x2 y2 x y -- x1 y1 x2 y2 x y d )
	swap >r >r 2swap 2dup r> swap - r> rot - ;

test


1 3 1 1 -1 -1 2 -3 3 0 5  .s graham .s