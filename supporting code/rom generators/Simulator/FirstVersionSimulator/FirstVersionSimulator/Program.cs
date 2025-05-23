
//a=0 b=1 c=a+b  b->a c->b

using System.Diagnostics;

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
Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

for (int i = 0; i < 3000000; i++)
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
stopwatch.Stop();
Console.WriteLine(p.registers[0]);
Console.WriteLine(p.registers[1]);
Console.WriteLine(p.registers[2]);
Console.WriteLine(stopwatch.ElapsedMilliseconds);


/*
 float worldSDF(vec3 pos)
{
    return min(pos.y,length(vec3(0,0,5)-pos)-1.0f);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = fragCoord/iResolution.xy;
    uv.x-=0.5f;
    uv.y-=0.5f;
    uv.x*=iResolution.x/iResolution.y;
    
    vec3 rayDir=vec3(uv.x,uv.y,1);
    vec3 rayPos=vec3(0,1,0);
    
    rayDir=normalize(rayDir);
    
    vec3 col = 0.5 + 0.5*cos(iTime+uv.xyx+vec3(0,2,4));
    for(int i=0;i<20;i++)
    {
        float maxStep=worldSDF(rayPos);
        rayPos+=rayDir*maxStep;
        if(maxStep<0.01f)
        {
            col=vec3(0,0,0);
                i=10;
        }
        
    }
    

    // Output to screen
    fragColor = vec4(col,1.0);
}*/