
//a=0 b=1 c=a+b  b->a c->b

ushort[] program = 
{
0b01000000,0,//load r0 with 0 working
0b101000000,1,//load r1 with 1 working

//label loop:
0b1001000011,//mov ro to r2
0b101000000000,//add r2 r1
0b10001011,12,//brc after loop
0b100001000011,//mov r1 to r0
0b1000101000011,//mov r2 to r1
0b10000000,4,//jmp to loop
//label after loop:
//jmp to after loop
0b10000000,12
};


Processor p = new Processor(program);

p.resetProcessor();

Console.WriteLine("starting ");
for (int i = 0; i < 20000; i++)
{
    p.risingClock();
    p.fallingClock();
   // Console.Write("Sub instruction: "+p.subInstructionCounter+" R=");
    for (int c = 0; c < 16; c++)
    { 
        //Console.Write(p.registers[c]+" ");
    }
    //Console.WriteLine();

}
Console.WriteLine(p.registers[0]);
/*Console.WriteLine(p.registers[1]);
Console.WriteLine(p.registers[2]);

*/
