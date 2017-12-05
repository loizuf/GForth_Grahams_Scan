\ determines wether Point (e,f) is to the left or to the right of the line between Point (a,b) and Point (c,d)
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

\ solved with locals		 
\ : whichSide ( x1 y1 x2 y2 x y -- x1 y1 x2 y2 x y d )
\	swap >r >r 2swap 2dup r> swap - r> rot - ;

\ saves all points in "points"
: readPoints ( x1 y1 x2 y2 ... xn yn pointsadr length -- pointsadr length)
	2 * dup
	0 u+do
		\ push the length on the return stack, arrange the next coordinate in the correct way (value adr) then take the length from the return stack, substract the loop counter and add that to the address. In this way we save the point in the correct order (x1 y1 x2 y2) 
		>r swap over r> i i . over >r - cells + ! r>
	loop ;

0 Value fd-in
     0 Value fd-out
     : open-input ( addr u -- )  r/o open-file throw to fd-in ;
     : open-output ( addr u -- )  w/o create-file throw to fd-out ;

: test
	10 0 u+do
		i .
	loop ;

1 2 3 4 5 6 7 8 9 0 5
create length , 
create points length @ 2 * cells allot 
points length @ readPoints