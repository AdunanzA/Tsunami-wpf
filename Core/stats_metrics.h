#pragma once

#include <libtorrent/session_stats.hpp>


namespace Tsunami
{
	namespace Core
	{

		public ref class StatsMetrics
		{
		public:
			StatsMetrics(libtorrent::stats_metric & metrics);
			~StatsMetrics();
		
			enum class enum_type
			{
				type_counter, 
				type_gauge
			};

		private:
			libtorrent::stats_metric * metrics_;

			property System::String ^ name { System::String ^ get(); }
			property int value_index { int get(); }
			property  int type { int get(); }
		};
	}
}