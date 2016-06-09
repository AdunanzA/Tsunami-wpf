#include "error_code.h"

#include "interop.h"

using namespace Tsunami::Core;

ErrorCode::ErrorCode(const libtorrent::error_code& ec)
{
    error_ = new libtorrent::error_code(ec);
}

ErrorCode::~ErrorCode()
{
    delete error_;
}

System::String^ ErrorCode::message()
{
    return interop::from_std_string(error_->message());
}

int ErrorCode::value()
{
    return error_->value();
}
