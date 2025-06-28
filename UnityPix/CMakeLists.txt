cmake_minimum_required(VERSION 3.0...3.25)

project(
        UnityPix
        VERSION 0.1.0
        LANGUAGES CXX C
)

add_subdirectory(src/Dx12)
add_subdirectory(src/WinPixEventRuntime)
add_subdirectory(src/UnityHeaders)

add_library(
        UnityPix SHARED
        src/main.cpp
)

target_link_libraries(UnityPix PRIVATE
        UnityPix_Dx12
        UnityPix_WinPixEventRuntime
        UnityPix_UnityHeaders
)

set_target_properties(UnityPix PROPERTIES
        CXX_STANDARD 20
        CXX_STANDARD_REQUIRED ON
        CXX_EXTENSIONS OFF
        COMPILE_WARNING_AS_ERROR ON
)

target_compile_definitions(UnityPix PRIVATE
        USE_PIX
)

if (MSVC)
    target_link_options(UnityPix PRIVATE
            /DEF:${CMAKE_CURRENT_SOURCE_DIR}/src/main.def
    )

    SET(TARGET_DLL_DIR "${CMAKE_CURRENT_SOURCE_DIR}/../Assets/Plugins/UnityPix/x64")
    add_custom_command(TARGET UnityPix POST_BUILD
            COMMAND ${CMAKE_COMMAND} -E copy
            $<TARGET_FILE:UnityPix>
            ${TARGET_DLL_DIR})
endif ()