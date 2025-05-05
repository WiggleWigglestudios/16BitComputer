
long[][] instructionFlags=new long[256][8];

int instructionOn=0;
int subInstructionOn=0;
long currentInstructionValue=0;

String[] labels=
{
  "r0toA",
  "r1toA",
  "r2toA",
  "r3toA",
  "r4toA",
  "r5toA",
  "r6toA",
  "r7toA",
  "PCtoA",
  "SPtoA",
  "STATtoA",
  "IRtoA",
  "MARtoA",
  "r13toA",
  "r14toA",
  "IMRtoA",
  
  "r0toB",
  "r1toB",
  "r2toB",
  "r3toB",
  "r4toB",
  "r5toB",
  "r6toB",
  "r7toB",
  "PCtoB",
  "SPtoB",
  "STATtoB",
  "IRtoB",
  "MARtoB",
  "r13toB",
  "r14toB",
  "IMRtoB",
  
  "Ctor0",
  "Ctor1",
  "Ctor2",
  "Ctor3",
  "Ctor4",
  "Ctor5",
  "Ctor6",
  "Ctor7",
  "CtoPC",
  "CtoSP",
  "CtoSTAT",
  "CtoIR",
  "CtoMAR",
  "Ctor13",
  "Ctor14",
  
  
  "AtoC",
  "BtoC",
  "ALUtoC",
  
  "AtoD",
  "BtoD",
  "DtoC",
  
  "incPC",
  "incSP",
  "decSP",
  "PCtoAddress",
  "MARtoAddress",
  "SPtoAddress",
  "ALUtoStat",
  "CtoPCC", //C to program counter conditional, only if condition is met
  "resetSTAT", //resets status register to before interupts
  "none",
  "end"  

};

String[] instructionNames=new String[256];


void getInstructionNames()
{
  instructionNames=loadStrings("InstructionNames.csv");
}

void writeInstructionFlags()
{
  byte[] data=new byte[256*8*8];
  for(int i=0;i<256;i++)
  {
    for(int c=0;c<8;c++)
    {
      for(int d=0;d<8;d++)
      {
          data[i*8*8+c*8+d]=(byte)(instructionFlags[i][c]>>>((d)*8));
      }
    }
  }
  
  println("saving file");
  saveBytes("instructionFlags.bin",data);
}


void loadInstructionFlags()
{
  byte[] data=loadBytes("instructionFlags.bin");
  if(data==null)
  {
    println("creating file");
    writeInstructionFlags();
  }
  else
  {
    println("loading file");
    for(int i=0;i<256;i++)
    {
      for(int c=0;c<8;c++)
      {
        for(int d=0;d<8;d++)
        {
          instructionFlags[i][c]=(instructionFlags[i][c]<<8)|((long)data[i*8*8+c*8+7-d]);
        }
      }
    }
  }
}

void updateCurrentInstruction()
{
  currentInstructionValue=instructionFlags[instructionOn][subInstructionOn];
}


void setup() {
  size(64*2*10, 20+100+10);
  loadInstructionFlags();
  updateCurrentInstruction();
  getInstructionNames();
}
 
void draw() {    
  background(255,255,255);
  strokeWeight(2);
  stroke(0,0,0);
  textSize(15);
  for(int i=0;i<64;i++)
  {
    float currentColor=0;
    long bit=currentInstructionValue&(((long)1)<<i);
    if(bit!=0)
    {
      currentColor=255;
    }
    
    
    if(mouseX>i*20&&mouseX<i*20+20&&mouseY<20)
    {
      currentColor=(127+currentColor)/2;
    }
    
    fill(currentColor,currentColor,currentColor);
    rect(i*20,0,20,20);
    
    fill(0,0,0);
    pushMatrix();
    translate(i*20+5,25);
    rotate(-30);
    text(labels[i],0,0);
    popMatrix();
  }
  
  instructionFlags[instructionOn][subInstructionOn]=currentInstructionValue;
  
  textSize(20);
  fill(0,0,0);
  text("Instruction: "+instructionNames[instructionOn],10,95);
  text(instructionOn+", "+subInstructionOn,10,115);
  
  text("sub:",400,95);
  text("instruction:",340,115);
  fill(3, 119, 252);
  stroke(0,0,0);
  for(int i=0;i<3;i++)
  {
    float currentColor=0;
    int bit=((int)subInstructionOn)&((1)<<(2-i));
    if(bit!=0)
    {
      currentColor=1;
    }
    
    if(mouseX>i*20+450&&mouseX<i*20+20+450&&mouseY>75&&mouseY<95)
    {
      currentColor=(0.75+currentColor)/2;
    }
    
    fill(3*currentColor, 119*currentColor, 252*currentColor);
    rect(450+i*20,75,20,20);
  } 
  for(int i=0;i<8;i++)
  {
    float currentColor=0;
    int bit=((int)instructionOn)&((1)<<(7-i));
    if(bit!=0)
    {
      currentColor=1;
    }
    
    if(mouseX>i*20+450&&mouseX<i*20+20+450&&mouseY>100&&mouseY<120)
    {
      currentColor=(0.75+currentColor)/2;
    }
    fill(3*currentColor, 119*currentColor, 252*currentColor);
    rect(450+i*20,100,20,20);
  }
  

}

void mousePressed()
{
  if(mouseY<20)
  {
    currentInstructionValue=currentInstructionValue^(1L<<((long)(mouseX/20)));
  }
  if(mouseX>450&&mouseX<510&&mouseY>75&&mouseY<95)
  {
    subInstructionOn=subInstructionOn^(1<<(2-(mouseX-450)/20));
    updateCurrentInstruction();
  }
  
  if(mouseX>450&&mouseX<610&&mouseY>100&&mouseY<120)
  {
    instructionOn=instructionOn^(1<<(7-(mouseX-450)/20));
    updateCurrentInstruction();
  }
  
}
void keyPressed()
{
  if(keyCode==83)
  {
    writeInstructionFlags();
  }
}
