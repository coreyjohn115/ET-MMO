@echo off
setlocal enabledelayedexpansion

set PYXLL_CONFIG_EXP_FILE=!PYXLL_CONFIG_FILE:pyxll.cfg=pyxll_exp.cfg!
for /f "" %%i in ('where pythonw') do set python_path=%%i
move %PYXLL_CONFIG_FILE% %PYXLL_CONFIG_FILE%.bak
echo %PYXLL_CONFIG_EXP_FILE%
for /f "delims=" %%i in (%PYXLL_CONFIG_EXP_FILE%) do (
	set a=%%i
	IF "!a:~0,9!" == "work_path" (
		set a=work_path=%~dp0
	)
	IF "!a:~0,8!" == "dst_path" (
		set a=dst_path=%~dp0..\Excel\
	)
	set a=!a:USER_PYTHON_PATH=%python_path%!
	set a=!a:BLZ_PROJECT_SCRIPT=%~dp0\script\!
	echo !a!>>%PYXLL_CONFIG_FILE%
)

set VS_SETTING_EXP_FILE=!PYXLL_CONFIG_FILE:pyxll.cfg=..\.vscode\settings_exp.json!
set VS_SETTING_FILE=!PYXLL_CONFIG_FILE:pyxll.cfg=..\.vscode\settings.json!
echo %VS_SETTING_FILE%
cd.>%VS_SETTING_FILE%
for /f "delims=" %%i in (%VS_SETTING_EXP_FILE%) do (
	set a=%%i
	set a=!a:BLZ_PROJECT_SCRIPT=%~dp0\script!
	set a=!a:\=/!
	echo !a!>>%VS_SETTING_FILE%
)
pause