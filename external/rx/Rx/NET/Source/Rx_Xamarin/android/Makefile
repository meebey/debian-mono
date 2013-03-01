
all: MonoDroid.Dialog Andr.Unit

MonoDroid.Dialog:
	git clone git://github.com/atsushieno/MonoDroid.Dialog.git

Andr.Unit:
	git clone git://github.com/pjcollins/Andr.Unit.git
	cd Andr.Unit && git checkout  bae4bbfc && patch -i ../andr-unit-rx.patch -p1 || exit 1


