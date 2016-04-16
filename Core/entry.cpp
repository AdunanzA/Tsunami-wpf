#include "entry.h"

#include "interop.h"


using namespace Tsunami::Core;

Entry::Entry(const libtorrent::entry& e)
{
    entry_ = new libtorrent::entry(e);
}

Entry::~Entry()
{
    delete entry_;
}

Entry::Entry(System::Collections::Generic::Dictionary<System::String^, Entry^> ^ value)
{
	libtorrent::entry::dictionary_type dictionary;
	
	for each (System::Collections::Generic::KeyValuePair<System::String ^, Entry ^> v in value)
	{
		dictionary[interop::to_std_string(v.Key)] = *v.Value->ptr();
	}

	entry_ = new libtorrent::entry(dictionary);
}

Entry::Entry(System::String ^ value)
{
	entry_ = new libtorrent::entry(interop::to_std_string(value));
}

Entry::Entry(System::Collections::Generic::List<Entry^> ^ value)
{
	libtorrent::entry::list_type entry_list;
		
	for each (Entry ^ v in value)
	{
		entry_list.push_back(*(v->ptr()));
	}

	entry_ = new libtorrent::entry(entry_list);
}

Entry::Entry(int value)
{
	entry_ = new libtorrent::entry(value);
}

libtorrent::entry* Entry::ptr()
{
    return entry_;
}
