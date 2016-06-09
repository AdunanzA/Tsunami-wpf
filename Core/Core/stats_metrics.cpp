#include "stats_metrics.h"
#include "interop.h"

using namespace Tsunami::Core;


StatsMetrics::StatsMetrics(libtorrent::stats_metric & metrics)
{
	metrics_ = new libtorrent::stats_metric(metrics);
}

StatsMetrics::~StatsMetrics()
{
	delete metrics_;
}

System::String ^ StatsMetrics::name::get()
{
	return interop::from_std_string(metrics_->name);
}

int StatsMetrics::value_index::get()
{
	return metrics_->value_index; 
}

int StatsMetrics::type::get()
{
	return metrics_->type;
}

