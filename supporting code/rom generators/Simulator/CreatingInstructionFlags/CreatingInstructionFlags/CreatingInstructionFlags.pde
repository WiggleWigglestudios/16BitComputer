
long[][] instructionFlags=new long[256][8];

int instructionOn=0;
int subInstructionOn=0;
long currentInstructionValue=0;

String[] labels=
{
  "RNtoA",
  "PCtoA",
  "RNtoB",
  "IMRroB",
  "CtoRN",
  "CtoPC",
  "CtoIR",
  "CtoMAR",
  "CtoIMR",
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
  "ALUtoSTAT",
  "CtoPCC",
  "resetSTAT",
  "END",
  "PCtoA",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",
  "none",

};

String[] instructionNames=new String[256];


void getInstructionNames()
{
  
  String[] inputNames=loadStrings("InstructionNames.csv");
  for(int i=0;i<256;i++)
  {
    if(i<inputNames.length)
    {
      instructionNames[i]=inputNames[i];
    }else
    {
      instructionNames[i]="";
    }
    
  }
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
          instructionFlags[i][c]=(instructionFlags[i][c]<<8)|(data[i*8*8+c*8+7-d]&0xFF);
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
  size(64*2*10, 20+200+10);
  loadInstructionFlags();
  updateCurrentInstruction();
  getInstructionNames();
}
 
void draw() {    
  background(255,255,255);
  strokeWeight(2);
  stroke(255,255,255);
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
    rect(i*20,-5,20,25);
    
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
  text("Instruction: "+instructionNames[instructionOn],10,195);
  text(instructionOn+", "+subInstructionOn,10,215);
  
  text("sub:",400,195);
  text("instruction:",340,215);
  fill(3, 119, 252);
  stroke(255,255,255);
  for(int i=0;i<3;i++)
  {
    float currentColor=0;
    int bit=((int)subInstructionOn)&((1)<<(2-i));
    if(bit!=0)
    {
      currentColor=1;
    }
    
    if(mouseX>i*20+450&&mouseX<i*20+20+450&&mouseY>175&&mouseY<195)
    {
      currentColor=(0.75+currentColor)/2;
    }
    
    fill(3*currentColor, 119*currentColor, 252*currentColor);
    rect(450+i*20,175,20,20);
  } 
  for(int i=0;i<8;i++)
  {
    float currentColor=0;
    int bit=((int)instructionOn)&((1)<<(7-i));
    if(bit!=0)
    {
      currentColor=1;
    }
    
    if(mouseX>i*20+450&&mouseX<i*20+20+450&&mouseY>200&&mouseY<220)
    {
      currentColor=(0.75+currentColor)/2;
    }
    fill(3*currentColor, 119*currentColor, 252*currentColor);
    rect(450+i*20,200,20,20);
  }
  
    
  fill(0, 255, 179);
  if(mouseX>10&&mouseX<96&&mouseY>128&&mouseY<158)
  {
     fill(127,255,220);
  }
  rect(10,128,86,30,2);
  fill(0,0,0);
  text("Set Fetch",15,150);
  
  
  
  
    
  fill(0, 255, 179);
  if(mouseX>100&&mouseX<210&&mouseY>128&&mouseY<158)
  {
     fill(127,255,220);
  }
  rect(100,128,110,30,2);
  fill(0,0,0);
  text("Set All Fetch",105,150);
  
  
  
  
  fill(0, 255, 179);
  if(mouseX>10&&mouseX<96&&mouseY>95&&mouseY<125)
  {
     fill(127,255,220);
  }
  rect(10,95,86,30,2);
  fill(0,0,0);
  text("Clear",15,118);
  
  
    
  fill(0, 255, 179);
  if(mouseX>100&&mouseX<210&&mouseY>95&&mouseY<125)
  {
     fill(127,255,220);
  }
  rect(100,95,110,30,2);
  fill(0,0,0);
  text("Repeat",105,118);
}

void mousePressed()
{
  //instruction flag buttons
  if(mouseY<20)
  {
    currentInstructionValue=currentInstructionValue^(1L<<((long)(mouseX/20)));
  }
  //sub instruction select
  if(mouseX>450&&mouseX<510&&mouseY>175&&mouseY<195)
  {
    subInstructionOn=subInstructionOn^(1<<(2-(mouseX-450)/20));
    updateCurrentInstruction();
  }
  
  //instruction select
  if(mouseX>450&&mouseX<610&&mouseY>200&&mouseY<220)
  {
    instructionOn=instructionOn^(1<<(7-(mouseX-450)/20));
    updateCurrentInstruction();
  }
  
  //set fetch
  if(mouseX>10&&mouseX<96&&mouseY>128&&mouseY<158)
  {
     currentInstructionValue=0b1000100000001000000;
  }
  
  //set all fetch
  if(mouseX>100&&mouseX<210&&mouseY>128&&mouseY<158)
  {
     for(int i=0;i<256;i++)
     {
       instructionFlags[i][0]=0b1000100000001000000;
     }
     updateCurrentInstruction();
  }
  
  //clear
  if(mouseX>10&&mouseX<96&&mouseY>95&&mouseY<125)
  {
    currentInstructionValue=0;
  }
  
    
  //repeat
  if(mouseX>100&&mouseX<210&&mouseY>95&&mouseY<125)
  {
    for(int i=instructionOn+4;i<256;i+=4)
    {
      for(int c=0;c<8;c++)
      {
        instructionFlags[i][c]=instructionFlags[instructionOn][c];
      }
    }
  }
  
}
void keyPressed()
{
  if(keyCode==83)
  {
    writeInstructionFlags();
  }
}
