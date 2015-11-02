struct DirChildren
{
	int MurmurHash;
    unsigned  __int64  Child as AbstractFile*;
};


struct File 
{

    int FileNameLenth;
	unsigned char Hash[32] ; //sha256
	wchar_t Name[FileNameLenth];
	
} ;

struct Dir
{
	int FileNameLenth;
	int RecordsCount;
	unsigned char hash[32];  //sha256
	wchar_t Name[FileNameLenth];
	DirChildren Children[RecordsCount];
};


struct GGPKFile
{
	int RecordsCount;
    unsigned  __int64  PROOT as Root * ;
	unsigned  __int64  PFree as AbstractFile*;
};

 struct AbstractFile   
{
	int Length;
	char Tag[4];
	case_union
    {
		 case Tag == "FILE":
		       File File;
		  case Tag == "PDIR":
			   Dir Dir;
		 case Tag == "GGPK":
		       GGPKFile GGPK;
		 case Tag == "FREE":
			 unsigned  __int64  NextFreeFile;// as AbstractFile*;
			// char Trash[Length - 16];
	 
	} AbstractFile;
};


 public struct GGPK  
{
	AbstractFile gpk;
};


public struct FileData
{
	int RowsCount;
};





struct Root
{
	AbstractFile Root;
};
