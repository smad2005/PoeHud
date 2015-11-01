
struct File 
{

    int FileNameLenth;
	unsigned char hash[32];
	wchar_t Name[FileNameLenth];
	
} ;

struct DirChildren
{
	int MurmurHash;
    unsigned  __int64  child as AbstractFile*;
};


struct Dir
{
	int FileNameLenth;
	int RecordsCount;
	unsigned char hash[32];
	wchar_t Name[FileNameLenth];
	DirChildren Children[RecordsCount];
};

struct GGPKFile
{
	int RecordsCount;
    unsigned  __int64  PROOT as Root * ;
	unsigned  __int64  PFree;
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
	 
	} AbstractFile;
};


 public struct GGPK  
{
	AbstractFile gpk;
};


public struct FileData
{
	
};





struct Root
{
	AbstractFile Root;
};
