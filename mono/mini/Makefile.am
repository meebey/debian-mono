count=100000
mtest=for_loop
monodir=$(top_builddir)

# This is needed for automake dependency generation
if INCLUDED_LIBGC
libgc_libs=$(monodir)/libgc/libmonogc.la
libgc_static_libs=$(monodir)/libgc/libmonogc-static.la
else
libgc_libs=$(LIBGC_LIBS)
libgc_static_libs=$(LIBGC_STATIC_LIBS)
endif

libs=	\
	$(monodir)/mono/metadata/libmonoruntime.la	\
	$(monodir)/mono/io-layer/libwapi.la	\
	$(monodir)/mono/utils/libmonoutils.la \
	$(GLIB_LIBS)	\
	$(libgc_libs)

sgen_libs = \
	$(monodir)/mono/metadata/libmonoruntimesgen.la	\
	$(monodir)/mono/io-layer/libwapi.la	\
	$(monodir)/mono/utils/libmonoutils.la \
	$(GLIB_LIBS)

if MOONLIGHT
moon_libs = \
	$(monodir)/mono/metadata/libmonoruntimemoon.la	\
	$(monodir)/mono/io-layer/libwapi.la	\
	$(monodir)/mono/utils/libmonoutils.la \
	$(GLIB_LIBS)
endif

static_libs=	\
	$(monodir)/mono/metadata/libmonoruntime-static.la	\
	$(monodir)/mono/io-layer/libwapi.la	\
	$(monodir)/mono/utils/libmonoutils.la \
	$(GLIB_LIBS)	\
	$(libgc_static_libs)

sgenstatic_libs = \
	$(monodir)/mono/metadata/libmonoruntimesgen-static.la	\
	$(monodir)/mono/io-layer/libwapi.la	\
	$(monodir)/mono/utils/libmonoutils.la \
	$(GLIB_LIBS)

CLASS=$(mcs_topdir)/class/lib/net_2_0

RUNTIME_EXECUTABLE = $(if $(SGEN),$(top_builddir)/mono/mini/mono-sgen,$(top_builddir)/runtime/mono-wrapper)

RUNTIME = MONO_PATH=$(CLASS) $(RUNTIME_EXECUTABLE)
RUNTIME_AOTCHECK = MONO_PATH=$(CLASS):. $(RUNTIME_EXECUTABLE)

MCS = $(RUNTIME) $(CLASS)/gmcs.exe -unsafe -nowarn:0162
ILASM = $(RUNTIME) $(CLASS)/ilasm.exe

AM_CFLAGS = \
	-I$(top_srcdir) 	\
	$(GLIB_CFLAGS)		\
	$(LLVM_CFLAGS)		\
	$(PLATFORM_CFLAGS) $(ARCH_CFLAGS)

AM_CXXFLAGS = $(LLVM_CXXFLAGS) $(GLIB_CFLAGS)

if HOST_WIN32
export HOST_CC
# The mingw math.h has "extern inline" functions that dont appear in libs, so
# optimisation is required to actually inline them
PLATFORM_CFLAGS = -O
endif

# hack for automake to have the same source file in a library and a bin
genmdesc_CFLAGS = $(AM_CFLAGS)

if NO_VERSION_SCRIPT
monoldflags=$(export_ldflags)
monobinldflags=$(export_ldflags)
else
monoldflags=-Wl,-version-script=$(srcdir)/ldscript $(export_ldflags)
monobinldflags=-Wl,-version-script=$(srcdir)/ldscript.mono $(export_ldflags)
endif

if HOST_WIN32
libmono_2_0_la_LDFLAGS=-no-undefined -avoid-version -Wl,--kill-at $(monoldflags)
else
libmono_2_0_la_LDFLAGS=$(monoldflags) -version-info 1:0:0
endif

if JIT_SUPPORTED

if SUPPORT_SGEN
sgen_binaries = mono-sgen
sgen_libraries = libmonosgen-2.0.la
sgen_static_libraries = libmonosgen-static.la
endif

if SUPPORT_BOEHM
boehm_libraries = libmono-2.0.la
boehm_binaries  = mono
endif

if HOST_WIN32
bin_PROGRAMS = mono monow
else
bin_PROGRAMS = $(boehm_binaries) $(sgen_binaries)
endif

noinst_PROGRAMS = genmdesc

if SHARED_MONO
shared_libraries = $(boehm_libraries) $(sgen_libraries)
endif

lib_LTLIBRARIES = $(shared_libraries)
noinst_LTLIBRARIES = libmono-static.la $(sgen_static_libraries)

if MOONLIGHT
noinst_LTLIBRARIES += libmono-moon.la
moon-do-build: $(BUILT_SOURCES) mono libmono-moon.la
moon-do-clean: maintainer-clean-generic
	-test -z "mono$(EXEEXT)" || rm -f mono$(EXEEXT)
	-test -z "libmono-moon.la" || rm -f libmono-moon.la
endif

if LOADED_LLVM
lib_LTLIBRARIES += libmono-llvm.la
libmono_llvm_la_SOURCES = mini-llvm.c mini-llvm-cpp.cpp
libmono_llvm_la_LIBADD = $(LLVM_LIBS) $(LLVM_LDFLAGS)
if PLATFORM_DARWIN
libmono_llvm_la_LDFLAGS=-undefined suppress
else
libmono_llvm_la_LIBADD += $(top_builddir)/mono/mini/libmono-$(API_VER).la $(libs)
endif
endif

endif

mono_SOURCES = \
	main.c

mono_CFLAGS = $(AM_CFLAGS) $(BOEHM_DEFINES) $(LIBGC_CFLAGS)

mono_sgen_SOURCES = $(mono_SOURCES)
mono_sgen_CFLAGS = $(SGEN_DEFINES) $(AM_CFLAGS)

# We build this after libmono was built so it contains the date when the final
# link was done
buildver.h: libmono-static.la
	@echo "const char *build_date;" > buildver.h

mono-main.$(OBJEXT): buildver.h
mono_sgen-main.$(OBJEXT): buildver.h

if DTRACE_G_REQUIRED
LIBMONO_DTRACE_OBJECT = .libs/mono-dtrace.$(OBJEXT)
if STATIC_MONO
MONO_DTRACE_OBJECT = mono-dtrace.$(OBJEXT)
else
MONO_DTRACE_OBJECT = 
endif
else
MONO_DTRACE_OBJECT = 
LIBMONO_DTRACE_OBJECT = 
endif

if STATIC_MONO
# Link libmono into mono statically
# This leads to higher performance, especially with TLS
MONO_LIB=libmono-static.la
MONO_SGEN_LIB=libmonosgen-static.la
else 
MONO_LIB=libmono-2.0.la
MONO_SGEN_LIB=libmonosgen-2.0.la
endif

if LOADED_LLVM
LLVMMONOF=
else
LLVMMONOF=$(LLVM_LIBS) $(LLVM_LDFLAGS)
endif

mono_LDADD = \
	$(MONO_LIB)		\
	$(GLIB_LIBS)		\
	$(LLVMMONOF)		\
	-lm			\
	$(MONO_DTRACE_OBJECT)

mono_LDFLAGS = \
	$(static_flags) -export-dynamic $(monobinldflags) $(monobin_platform_ldflags)

mono_sgen_LDADD = \
	$(MONO_SGEN_LIB)	\
	$(GLIB_LIBS)		\
	$(LLVMMONOF)		\
	-lm			\
	$(MONO_DTRACE_OBJECT)

mono_sgen_LDFLAGS = $(static_flags) -export-dynamic $(monobinldflags) $(monobin_platform_ldflags)

if DTRACE_G_REQUIRED

mono-dtrace.$(OBJEXT): $(top_srcdir)/data/mono.d mini.lo $(monodir)/mono/metadata/libmonoruntime-static.la
	DTRACE="$(DTRACE)" DTRACEFLAGS="$(DTRACEFLAGS)" AR="$(AR)" $(SHELL) $(top_srcdir)/data/dtrace-prelink.sh \
	$@ $(top_srcdir)/data/mono.d $(monodir)/mono/metadata/libmonoruntime-static.la mini.lo

.libs/mono-dtrace.$(OBJEXT): $(top_srcdir)/data/mono.d mini.lo $(monodir)/mono/metadata/libmonoruntime.la
	DTRACE="$(DTRACE)" DTRACEFLAGS="$(DTRACEFLAGS)" AR="$(AR)" $(SHELL) $(top_srcdir)/data/dtrace-prelink.sh \
	--pic $@ $(top_srcdir)/data/mono.d $(monodir)/mono/metadata/libmonoruntime.la mini.lo

endif

# Create monow.exe, linked for the 'windows' subsystem
if HOST_WIN32
monow_LDADD = $(mono_LDADD)
monow_LDFLAGS = $(mono_LDFLAGS) -mwindows
monow_SOURCES = $(mono_SOURCES)
endif

genmdesc_SOURCES = \
	mini.h		\
	genmdesc.c	\
	helpers.c	\
	../metadata/opcodes.c

# Don't link this against libmonoruntime to speed up rebuilds
genmdesc_LDADD = \
	$(monodir)/mono/utils/libmonoutils.la -lm	\
	$(GLIB_LIBS)

x86_sources = \
	mini-x86.c		\
	mini-x86.h		\
	exceptions-x86.c	\
	tramp-x86.c

amd64_sources = \
	mini-amd64.c		\
	mini-amd64.h		\
	exceptions-amd64.c	\
	tramp-amd64.c

ppc_sources = \
	mini-ppc.c		\
	mini-ppc.h		\
	exceptions-ppc.c	\
	tramp-ppc.c

arm_sources = \
	mini-arm.c		\
	mini-arm.h		\
	exceptions-arm.c	\
	tramp-arm.c

mips_sources = \
	mini-mips.c		\
	mini-mips.h		\
	exceptions-mips.c	\
	tramp-mips.c

sparc_sources = \
	mini-sparc.c		\
	mini-sparc.h		\
	exceptions-sparc.c	\
	tramp-sparc.c

s390_sources = \
	mini-s390.c		\
	mini-s390.h		\
	exceptions-s390.c	\
	tramp-s390.c

s390x_sources = \
	mini-s390x.c		\
	mini-s390x.h		\
	exceptions-s390x.c	\
	tramp-s390x.c

ia64_sources = \
	mini-ia64.c		\
	mini-ia64.h		\
	exceptions-ia64.c	\
	tramp-ia64.c

alpha_sources = \
	mini-alpha.c		\
	mini-alpha.h		\
	exceptions-alpha.c	\
	tramp-alpha.c

hppa_sources = \
	mini-hppa.c		\
	mini-hppa.h		\
	exceptions-hppa.c	\
	tramp-hppa.c

darwin_sources = \
	mini-darwin.c

windows_sources = \
	mini-windows.c

posix_sources = \
	mini-posix.c

if ENABLE_LLVM
if LOADED_LLVM
llvm_sources =
else
llvm_sources = \
	mini-llvm.c		\
	mini-llvm-cpp.cpp
endif
endif

common_sources = \
	mini.c			\
	ir-emit.h		\
	method-to-ir.c		\
	decompose.c		\
	mini.h			\
	version.h		\
	optflags-def.h		\
	jit-icalls.h 		\
	jit-icalls.c 		\
	trace.c			\
	trace.h			\
	patch-info.h		\
	mini-ops.h		\
	mini-arch.h		\
	dominators.c		\
	cfold.c			\
	regalloc.c		\
	regalloc.h		\
	helpers.c		\
	liveness.c		\
	ssa.c			\
	abcremoval.c		\
	abcremoval.h		\
	ssapre.c		\
	ssapre.h		\
	local-propagation.c	\
	driver.c		\
	debug-mini.c		\
	debug-mini.h		\
	linear-scan.c		\
	aot-compiler.c		\
	aot-runtime.c		\
	graph.c			\
	mini-codegen.c		\
	mini-exceptions.c	\
	mini-trampolines.c  	\
	declsec.c		\
	declsec.h		\
	wapihandles.c		\
	branch-opts.c		\
	mini-generic-sharing.c	\
	regalloc2.c		\
	simd-methods.h		\
	tasklets.c		\
	tasklets.h		\
	simd-intrinsics.c	\
	mini-unwind.h		\
	unwind.c		\
	image-writer.h		\
	image-writer.c		\
	dwarfwriter.h		\
	dwarfwriter.c		\
	mini-gc.h		\
	mini-gc.c		\
	debugger-agent.h 	\
	debugger-agent.c	\
	debug-debugger.c	\
	debug-debugger.h	\
	xdebug.c			\
	mini-llvm.h			\
	mini-llvm-cpp.h

test_sources = 			\
	basic-calls.cs 		\
	basic-long.cs 		\
	bench.cs 		\
	objects.cs 		\
	arrays.cs		\
	basic-float.cs		\
	basic-math.cs		\
	basic.cs		\
	exceptions.cs		\
	devirtualization.cs	\
	iltests.il.in		\
	test.cs			\
	generics.cs		\
	generics-variant-types.il\
	basic-simd.cs

regtests=basic.exe basic-float.exe basic-long.exe basic-calls.exe objects.exe arrays.exe basic-math.exe exceptions.exe iltests.exe devirtualization.exe generics.exe basic-simd.exe

if X86
if MONO_DEBUGGER_SUPPORTED
if PLATFORM_DARWIN
mdb_x86 = mdb-debug-info32-darwin.s
else
mdb_x86 = mdb-debug-info32.s
endif
else
mdb_x86 = 
endif
arch_sources = $(x86_sources) $(mdb_x86)
arch_built=cpu-x86.h
arch_define=__i386__
endif

if AMD64
if MONO_DEBUGGER_SUPPORTED
arch_sources = $(amd64_sources) mdb-debug-info64.s
else
arch_sources = $(amd64_sources)
endif
arch_built=cpu-amd64.h
arch_define=__x86_64__
endif

if POWERPC
arch_sources = $(ppc_sources)
arch_built=cpu-ppc.h
arch_define=__ppc__
endif

if POWERPC64
arch_sources = $(ppc_sources)
arch_built=cpu-ppc64.h
arch_define=__ppc64__
endif

if MIPS
arch_sources = $(mips_sources)
arch_built=cpu-mips.h
arch_define=__mips__
endif

if ARM
# pick up arm_dpimacros.h and arm_fpamacros.h
ARCH_CFLAGS = -I../arch/arm
arch_sources = $(arm_sources)
arch_built=cpu-arm.h
arch_define=__arm__
endif

if SPARC
arch_sources = $(sparc_sources)
arch_built=cpu-sparc.h
arch_define=__sparc__
endif

if SPARC64
arch_sources = $(sparc_sources)
arch_built=cpu-sparc.h
arch_define=__sparc__
endif

if S390
arch_sources = $(s390_sources)
arch_built=cpu-s390.h
arch_define=__s390__
endif

if S390x
arch_sources = $(s390x_sources)
arch_built=cpu-s390x.h
arch_define=__s390__
endif

if IA64
arch_sources = $(ia64_sources)
arch_built = cpu-ia64.h
arch_define=__ia64__
endif

if ALPHA
arch_sources = $(alpha_sources) $(mono_debugger_sources)
arch_built = cpu-alpha.h
arch_define=__alpha__
endif

if HPPA
# Only support 32-bit targets for now
arch_sources = $(hppa_sources)
arch_built = cpu-hppa.h
arch_define=__hppa__
endif

if HOST_WIN32
os_sources = $(windows_sources)
monobin_platform_ldflags=
endif

if PLATFORM_SIGPOSIX
os_sources = $(posix_sources)
monobin_platform_ldflags=
endif

if PLATFORM_DARWIN
os_sources = $(darwin_sources) $(posix_sources)
#monobin_platform_ldflags=-sectcreate __TEXT __info_plist $(top_srcdir)/mono/mini/Info.plist -framework CoreFoundation
monobin_platform_ldflags=-framework CoreFoundation
endif

libmono_2_0_la_SOURCES = $(common_sources) $(llvm_sources) $(arch_sources) $(os_sources)
libmono_2_0_la_CFLAGS = $(mono_CFLAGS)
libmono_2_0_la_LIBADD = $(libs) $(LIBMONO_DTRACE_OBJECT)

libmonosgen_2_0_la_SOURCES = $(libmono_2_0_la_SOURCES)
libmonosgen_2_0_la_CFLAGS = $(mono_sgen_CFLAGS)
libmonosgen_2_0_la_LIBADD = $(sgen_libs) $(LIBMONO_DTRACE_OBJECT)

if MOONLIGHT
libmono_moon_la_SOURCES = $(libmono_2_0_la_SOURCES)
if MOONLIGHT_BOEHM
libmono_moon_la_CFLAGS = $(mono_CFLAGS) $(MOONLIGHT_DEFINES)
libmono_moon_la_LIBADD = $(moon_libs) $(libgc_libs) $(LIBMONO_DTRACE_OBJECT)
else
libmono_moon_la_CFLAGS = $(mono_sgen_CFLAGS) $(MOONLIGHT_DEFINES)
libmono_moon_la_LIBADD = $(moon_libs) $(LIBMONO_DTRACE_OBJECT)
endif
endif

libmono_static_la_SOURCES = $(libmono_2_0_la_SOURCES)
libmono_static_la_CFLAGS = $(mono_CFLAGS)
libmono_static_la_LDFLAGS = -static
libmono_static_la_LIBADD = $(static_libs) $(MONO_DTRACE_OBJECT)

libmonosgen_static_la_SOURCES = $(libmono_2_0_la_SOURCES)
libmonosgen_static_la_CFLAGS = $(mono_sgen_CFLAGS)
libmonosgen_static_la_LDFLAGS = -static
libmonosgen_static_la_LIBADD = $(sgenstatic_libs) $(MONO_DTRACE_OBJECT)

nodist_libmono_static_la_SOURCES = $(nodist_libmono_la_SOURCES)
nodist_libmono_static_la_CFLAGS = $(mono_CFLAGS)

BURGSRC= $(common_BURGSRC) $(arch_BURGSRC)

libmonoincludedir = $(includedir)/mono-$(API_VER)/mono/jit

libmonoinclude_HEADERS = jit.h

basic-simd.exe: basic-simd.cs
	$(MCS) -out:$@ $< -r:TestDriver.dll -r:Mono.Simd.dll

nacl.exe: nacl.cs
	$(MCS) -out:$@ $< -r:TestDriver.dll -r:Mono.Simd.dll

generics.exe: generics.cs TestDriver.dll generics-variant-types.dll
	$(MCS) -out:$@ $< -r:TestDriver.dll -r:generics-variant-types.dll

%.exe: %.cs TestDriver.dll
	$(MCS) -out:$@ -unsafe $< -r:TestDriver.dll

iltests.il: iltests.il.in Makefile.am
	echo '// DO NOT EDIT: This file has been generated from iltests.il.in' > iltests.il
	cpp -Darch=$(arch_define) < $(srcdir)/iltests.il.in | sed 's/^#.*//' >> iltests.il

%.exe: %.il
	$(ILASM) -output=$@ $<

TestDriver.dll: $(srcdir)/TestDriver.cs
	$(MCS) -out:$@ -target:library $<

generics-variant-types.dll: generics-variant-types.il
	$(ILASM) -dll -output=$@ $<

if NACL_CODEGEN
GENMDESC_OPTS=--nacl
else !NACL_CODEGEN
GENMDESC_OPTS=
endif !NACL_CODEGEN

# we don't always use the perl impl because it's an additional
# build dependency for the poor windows users
# $(arch_define) is the preprocessor symbol that enables all the opcodes
# for the specific platform in mini-ops.h
if CROSS_COMPILING
GENMDESC_PRG=perl $(srcdir)/genmdesc.pl $(arch_define) $(srcdir) $(GENMDESC_OPTS)
else !CROSS_COMPILING
GENMDESC_PRG=./genmdesc $(GENMDESC_OPTS)
endif !CROSS_COMPILING

cpu-x86.h: cpu-x86.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-x86.h x86_desc $(srcdir)/cpu-x86.md

cpu-amd64.h: cpu-amd64.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-amd64.h amd64_desc $(srcdir)/cpu-amd64.md

cpu-ppc.h: cpu-ppc.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-ppc.h ppcg4 $(srcdir)/cpu-ppc.md

cpu-ppc64.h: cpu-ppc64.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-ppc64.h ppc64_cpu_desc $(srcdir)/cpu-ppc64.md

cpu-arm.h: cpu-arm.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-arm.h arm_cpu_desc $(srcdir)/cpu-arm.md

cpu-sparc.h: cpu-sparc.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-sparc.h sparc_desc $(srcdir)/cpu-sparc.md

cpu-s390.h: cpu-s390.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-s390.h s390_cpu_desc $(srcdir)/cpu-s390.md

cpu-s390x.h: cpu-s390x.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-s390x.h s390x_cpu_desc $(srcdir)/cpu-s390x.md

cpu-ia64.h: cpu-ia64.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-ia64.h ia64_desc $(srcdir)/cpu-ia64.md

cpu-alpha.h: cpu-alpha.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-alpha.h alpha_desc $(srcdir)/cpu-alpha.md

cpu-hppa.h: cpu-hppa.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-hppa.h hppa_desc $(srcdir)/cpu-hppa.md

cpu-mips.h: cpu-mips.md genmdesc$(EXEEXT)
	$(GENMDESC_PRG) cpu-mips.h mips_desc $(srcdir)/cpu-mips.md

testi: mono test.exe
	$(RUNTIME) -v -v --ncompile 1 --compile Test:$(mtest) test.exe

# ensure the tests are actually correct
checktests: $(regtests)
	for i in $(regtests); do $(RUNTIME) $$i; done

rcheck: mono $(regtests)
	$(RUNTIME) --regression $(regtests)

LLVM_AOT_RUNTIME_OPTS=$(if $(LLVM),--llvm,)

aotcheck: mono $(regtests)
	rm -f *.exe.so
	$(RUNTIME) $(LLVM_AOT_RUNTIME_OPTS) --aot $(regtests) || exit 1
	for i in $(regtests); do $(RUNTIME_AOTCHECK) --regression $$i || exit 1; done
	rm -f *.exe.so

llvmaotcheck:
	$(MAKE) aotcheck LLVM=1

# This currently only works on amd64/arm
fullaotcheck: mono $(regtests)
	rm -rf fullaot-tmp
	mkdir fullaot-tmp
	cp $(CLASS)/mscorlib.dll $(CLASS)/System.Core.dll $(CLASS)/System.dll $(CLASS)/Mono.Posix.dll $(CLASS)/System.Configuration.dll $(CLASS)/System.Security.dll $(CLASS)/System.Xml.dll $(CLASS)/Mono.Security.dll $(CLASS)/Mono.Simd.dll $(regtests) generics-variant-types.dll TestDriver.dll fullaot-tmp/
	cp $(regtests) fullaot-tmp/
	MONO_PATH=fullaot-tmp $(top_builddir)/runtime/mono-wrapper $(LLVM_AOT_RUNTIME_OPTS) --aot=full fullaot-tmp/* || exit 1
	for i in $(regtests); do echo $$i; MONO_PATH=fullaot-tmp $(top_builddir)/runtime/mono-wrapper --full-aot fullaot-tmp/$$i --exclude '!FULLAOT' || exit 1; done

llvmfullaotcheck:
	$(MAKE) fullaotcheck LLVM=1

bench: mono test.exe
	time env $(RUNTIME) --ncompile $(count) --compile Test:$(mtest) test.exe

mbench: test.exe
	time $(monodir)/mono/jit/mono --ncompile $(count) --compile Test:$(mtest) test.exe

stat1: mono bench.exe
	$(RUNTIME) --verbose --statfile stats.pl --regression bench.exe
	perl viewstat.pl stats.pl

stat2: mono basic.exe
	$(RUNTIME) --verbose --statfile stats.pl --regression basic.exe
	perl viewstat.pl -e stats.pl

stat3: mono bench.exe
	$(RUNTIME) --statfile stats.pl --ncompile 1000 --compile Tests:test_0_many_nested_loops bench.exe 
	perl viewstat.pl stats.pl

docu: mini.sgm
	docbook2txt mini.sgm

check-local: rcheck

clean-local:
	rm -f mono a.out gmon.out *.o buildver.h test.exe

pkgconfigdir = $(libdir)/pkgconfig

if JIT_SUPPORTED
BUILT_SOURCES = version.h $(arch_built)
else
BUILT_SOURCES = version.h
endif

CLEANFILES= $(BUILT_SOURCES) *.exe *.dll
EXTRA_DIST = $(common_BURGSRC) TestDriver.cs ldscript ldscript.mono \
	genmdesc.pl				\
	$(test_sources) 			\
	$(x86_sources) cpu-x86.md 		\
	$(amd64_sources) cpu-amd64.md 		\
	$(ppc_sources) cpu-ppc.md cpu-ppc64.md 	\
	$(arm_sources) cpu-arm.md 		\
	$(mips_sources) cpu-mips.md 		\
	$(sparc_sources) cpu-sparc.md 		\
	$(s390_sources) cpu-s390.md 		\
	$(s390x_sources) cpu-s390x.md 		\
	$(ia64_sources) cpu-ia64.md 		\
	$(alpha_sources) cpu-alpha.md 		\
	$(hppa_sources) cpu-hppa.md 		\
	$(windows_sources)			\
	$(darwin_sources) Info.plist		\
	$(posix_sources)

version.h: Makefile
	echo "#define FULL_VERSION \"Debian $$(dpkg-parsechangelog -l$(top_srcdir)/debian/changelog | grep ^Vers | cut -d\  -f2)\"" > version.h

# Utility target for patching libtool to speed up linking
patch-libtool:
	sed -e 's,if (for obj in $$oldobjs,if (for obj in "",g' < ../../libtool > 2; mv 2 ../../libtool
	chmod a+x ../../libtool

tags:
	etags -o TAGS `find .. -name "*.h" -o -name "*.c"`
