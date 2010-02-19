thisdir = tools

per_profile_dirs = \
	al		\
	compiler-tester	\
	linker		\
	tuner		\
	csharp		\
	culevel		\
	genxs		\
	mkbundle	\
	monop		\
	mono-service	\
	mono-xsd	\
	resgen		\
	wsdl

net_2_0_dirs := \
	$(per_profile_dirs) \
	cilc		\
	corcompare	\
	mono-xmltool	\
	xbuild		\
	monodoc		\
	mdoc		\
	mono-shlib-cop	\
	sgen		\
	mconfig		\
	installutil	\
	nunitreport	\
	pdb2mdb		\
	sqlsharp	\
	sqlmetal	\
	svcutil		\
	ictool		\
	disco		\
	soapsuds	\
	browsercaps-updater	\
	cil-strip	\
	macpack		\
	dtd2rng		\
	dtd2xsd		\
	mjs		\
	prj2make	\
	mod		\
	installvst	\
	lc		\
	mono-configuration-crypto

net_2_1_dirs := \
	compiler-tester

basic_SUBDIRS = gacutil security
net_2_0_bootstrap_SUBDIRS = resgen culevel
net_2_0_SUBDIRS := $(net_2_0_dirs) $(basic_SUBDIRS)
net_2_1_raw_SUBDIRS := $(net_2_1_dirs)
net_2_1_SUBDIRS := $(net_2_1_dirs)
net_4_0_bootstrap_SUBDIRS = resgen culevel
net_4_0_SUBDIRS := $(per_profile_dirs)

SUBDIRS = $(basic_SUBDIRS) $(net_2_0_bootstrap_SUBDIRS) $(net_2_0_SUBDIRS) $(net_2_1_SUBDIRS)

include ../build/rules.make

DISTFILES = \
	assemblies.xml			\
	mono-win32-setup-dark.bmp	\
	mono-win32-setup-light.bmp	\
	mono-win32-setup.nsi		\
	scan-tests.pl			\
	tinderbox/smtp.c		\
	tinderbox/tinderbox.sh		\
	gensources.sh			\
	removecomments.sh

test-local csproj-local run-test-local run-test-ondotnet-local all-local install-local uninstall-local doc-update-local:
	@:

dist-local: dist-default

clean-local:
	rm -f *.exe *.dll *.pdb cormissing.xml