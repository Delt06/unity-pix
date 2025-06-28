cmake_minimum_required(VERSION 3.0...3.25)

project(
        UnityPix_WinPixEventRuntime
        VERSION 0.1.0
        LANGUAGES CXX C
)

add_library(UnityPix_WinPixEventRuntime INTERFACE)

set_target_properties(UnityPix_WinPixEventRuntime PROPERTIES
        CXX_STANDARD 20
        CXX_STANDARD_REQUIRED ON
        CXX_EXTENSIONS OFF
        COMPILE_WARNING_AS_ERROR ON
)

target_include_directories(UnityPix_WinPixEventRuntime INTERFACE
        $<BUILD_INTERFACE:${CMAKE_CURRENT_LIST_DIR}/include/WinPixEventRuntime>)
