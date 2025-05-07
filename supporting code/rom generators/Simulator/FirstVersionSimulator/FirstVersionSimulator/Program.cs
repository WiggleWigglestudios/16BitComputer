
//a=0 b=1 c=a+b  b->a c->b

ushort[] program = 
{
0b01000000,0,//load r0 with 0
0b101000000,1,//load r1 with 1

//label loop:
0b1001000011,//mov ro to r2
0b101000000000,//add r2 r1
0b100001000011,//mov r1 to r0
0b1000101000011,//mov r2 to r1
0b10000000,4//jmp to loop

};


Processor p = new Processor(program);

p.resetProcessor();


for (int i = 0; i < 1000; i++)
{
    //Console.WriteLine(i);
    p.risingClock();
    p.fallingClock();
    for (int c = 0; c < 16; c++)
    { 
       // Console.Write(p.registers[c]+" ");
    }

   // Console.WriteLine(p.registers[0]);
}


