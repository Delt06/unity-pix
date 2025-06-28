cmake_minimum_required(VERSION 3.0...3.25)

project(
        UnityPix_UnityHeaders
        VERSION 0.1.0
        LANGUAGES CXX C
)

add_library(UnityPix_UnityHeaders INTERFACE)

set_target_properties(UnityPix_UnityHeaders PROPERTIES
        CXX_STANDARD 20
        CXX_STANDARD_REQUIRED ON
        CXX_EXTENSIONS OFF
        COMPILE_WARNING_AS_ERROR ON
)

target_include_directories(UnityPix_UnityHeaders INTERFACE
        $<BUILD_INTERFACE:${CMAKE_CURRENT_LIST_DIR}/include>)
