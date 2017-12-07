\ determines wether Point (e,f) is to the left or to the right of the line between Point (a,b) and Point (c,d)
: whichSideLocals { a b c d e f }
	a b c d e f e a - d b - * f b - c a - * - ;

: putFirstPoints ( pointsadr length - x1 y1 x2 y2 x3 y3 pointsadr length)
	>r
	6 0 u+do
		dup i cells + @ swap
	loop
	r> swap ; ( take length from return stack, swap it behind the pointadr)

: graham ( pointsadr length - CONVEXHULL POINTS )
	
	putFirstPoints

	7 >r ( we create a counter which is pushed on the return stack, we count how many times we already skipped a vertex until we found the first three)
	>r ( pointsadr pushed to returnstack)

	\ iterate over the rest
	3 u+do
		begin 
			.s whichSideLocals 0 < while
				2swap drop drop
		repeat

		r> r> r> r> ( take two loop parameters and pointadr and counter from return stack)
		dup 2 + >r ( count counter up by 2, and store copy on return stack)
		over >r ( store copy of pointadr back on return stack)
		cells + ( set address to next relevant point)
		dup 1 cells - ( we need this address 2 times, x- and y-coordinate)
		@ swap @ ( the Top address was one cell less and therefore the x adress so after this line a new point is placed on the stack)
		2swap >r >r ( take the two loop parameters and store them back on the return stack)

	loop
	r> r> drop drop ; ( take pointadr and counter from the return stack and drop them so that we get the same return stack at the end of the function as at the start)

\ saves all points in "points"
: readPoints ( x1 y1 x2 y2 ... xn yn pointsadr length -- pointsadr length)
	2 * dup
	0 u+do
		\ push the length on the return stack, arrange the next coordinate in the correct way (value adr) then take the length from the return stack, substract the loop counter and add that to the address. In this way we save the point in the correct order (x1 y1 x2 y2) 
		>r swap over r> i over >r - 1 - cells + ! r>
	loop 2 / ;

\ solved with locals		 
\ : whichSide ( x1 y1 x2 y2 x y -- x1 y1 x2 y2 x y d )
\	swap >r >r 2swap 2dup r> swap - r> rot - ;

: split ( str len separator len -- tokens count )
  here >r 2swap
  begin
    2dup 2,             \ save this token ( addr len )
    2over search        \ find next separator
  while
    dup negate  here 2 cells -  +!  \ adjust last token length
    2over nip /string               \ start next search past separator
  repeat
  2drop 2drop
  r>  here over -   ( tokens length )
  dup negate allot           \ reclaim dictionary
  2 cells / ;                \ turn byte length into token count
 
: intToken ( tokens count -- )
  \ 1 ?do dup 2@ s>number? cell+ cell+ loop 2@ type
  \ 1 ?do 2@ 2@ .s rot rot 2@ .s s>number? .s 2drop 2drop 2drop cell+ cell+ loop 2@ s>number? 2drop drop ;
  \ drop 
  \ 2@ s>number? 2drop rot rot cell+ cell+
  \ 2@ s>number? 2drop ;
  dup 2@ s>number? 2drop swap ( x tokens )
  cell+ cell+ 2@ s>number? 2drop ;

128 Constant max-line \ the maximum line length
Create line-buffer max-line 2 + allot \ read buffer
0 Value fd-in

: open-input ( addr u -- )  r/o open-file throw to fd-in ;

: loadPoints ( )
	s" in.txt" open-input
	begin
		line-buffer max-line fd-in read-line throw ( length not-eof-flag )
	while ( length )
		line-buffer swap s"  " split drop intToken
	repeat drop
	fd-in close-file throw ;

: test
	10 0 u+do
		i 1+ i .
	loop ;

0 0 0 3 1 4 1 2 3 5 6 4 3 1 7 0 8

\ number of points is on top of the stack, save it in length
create length , 

\ create an array to save the points in (since it has 2 coordinates its length*2 in length)
create points length @ 2 * cells allot 

\ pointadr length is put on the stack and array is filled
points length @ readPoints graham .s