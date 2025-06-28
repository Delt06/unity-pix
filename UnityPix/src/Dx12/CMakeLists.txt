cmake_minimum_required(VERSION 3.0...3.25)

project(
        UnityPix_Dx12
        VERSION 0.1.0
        LANGUAGES CXX C
)

add_library(UnityPix_Dx12 STATIC
        include/d3dx12/d3dx12.h
        include/d3dx12/d3dx12_barriers.h
        include/d3dx12/d3dx12_check_feature_support.h
        include/d3dx12/d3dx12_core.h
        include/d3dx12/d3dx12_default.h
        include/d3dx12/d3dx12_pipeline_state_stream.h
        include/d3dx12/d3dx12_property_format_table.h
        include/d3dx12/d3dx12_render_pass.h
        include/d3dx12/d3dx12_resource_helpers.h
        include/d3dx12/d3dx12_root_signature.h
        include/d3dx12/d3dx12_state_object.h

        include/d3d12.h
        include/d3d12compatibility.h
        include/d3d12sdklayers.h
        include/d3d12shader.h
        include/D3D12TokenizedProgramFormat.hpp
        include/d3d12video.h
        include/d3dcommon.h
        include/dxgiformat.h

        src/d3dx12/d3dx12_property_format_table.cpp
)

set_target_properties(UnityPix_Dx12 PROPERTIES
        CXX_STANDARD 20
        CXX_STANDARD_REQUIRED ON
        CXX_EXTENSIONS OFF
        COMPILE_WARNING_AS_ERROR ON
)

target_include_directories(UnityPix_Dx12 PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_LIST_DIR}/include>)
target_include_directories(UnityPix_Dx12 PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_LIST_DIR}/include/d3dx12>)
