import 'dart:convert';
import 'dart:math';

import 'package:audioplayers/audioplayers.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';


void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Laugh App',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.yellow),
      ),
      home: const MyHomePage(title: 'Random laughs for you'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key, required this.title});

  final String title;

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  int _imageIndex = 0;
  int _audioIndex = 0;
  var random = Random();
  var _images = List<String>.empty(growable: true) ;
  var _audioPaths = List<String>.empty(growable: true) ;
  final AudioPlayer _audioPlayer = AudioPlayer();
  
   _MyHomePageState() {
    _initImagesAndAudio();
   }

   GestureTapCallback? showNextImage() {
    _showNextImage();
    return null;
  }

Future _initImagesAndAudio() async {
      // >> To get paths you need these 2 lines
      final manifestContent = await rootBundle.loadString('AssetManifest.json');
    
      final Map<String, dynamic> manifestMap = json.decode(manifestContent);
      // >> To get paths you need these 2 lines

      final imagePaths = manifestMap.keys
          .where((String key) => key.contains('lib/images/'))
          .where((String key) => key.contains('.jpg') || key.contains('.png') || key.contains('.jpeg'))
          .toList();

      final audioPaths = manifestMap.keys
          .where((String key) => key.contains('assets/laugh_sounds/'))
          .where((String key) => key.contains('.mp3'))
          .toList();
    
      _images = imagePaths;
      _audioPaths = audioPaths;

      await _audioPlayer.play(_audioPaths.length > _audioIndex ? AssetSource(_audioPaths[_audioIndex].replaceFirst('assets/', '')) : AssetSource('laugh_sounds/baby_boy_laugh.mp3'));
    }

  void _showNextImage() async {
    var nextImageInt = -1;
    var nextAudioInt = -1;
    
    do {
      nextImageInt = random.nextInt(15);
    } while (nextImageInt == _imageIndex);

    do {
      nextAudioInt = random.nextInt(15);
    } while (nextAudioInt == _audioIndex);
    _audioIndex = nextAudioInt;
      
    await _audioPlayer.play(_audioPaths.length > _audioIndex ? AssetSource(_audioPaths[_audioIndex].replaceFirst('assets/', '')) : AssetSource('laugh_sounds/baby_boy_laugh.mp3'));

    setState(() {
      // This call to setState tells the Flutter framework that something has
      // changed in this State, which causes it to rerun the build method below
      // so that the display can reflect the updated values. If we changed
      // _counter without calling setState(), then the build method would not be
      // called again, and so nothing would appear to happen.
      //_counter = random.nextInt(15);

      
      _imageIndex = nextImageInt;
    });
  }

  @override
  Widget build(BuildContext context) {
    // This method is rerun every time setState is called, for instance as done
    // by the _incrementCounter method above.
    //
    // The Flutter framework has been optimized to make rerunning build methods
    // fast, so that you can just rebuild anything that needs updating rather
    // than having to individually change instances of widgets.
    return Scaffold(
      appBar: AppBar(
        // TRY THIS: Try changing the color here to a specific color (to
        // Colors.amber, perhaps?) and trigger a hot reload to see the AppBar
        // change color while the other colors stay the same.
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
        // Here we take the value from the MyHomePage object that was created by
        // the App.build method, and use it to set our appbar title.
        title: Text(widget.title),
      ),
      body: Center(
        // Center is a layout widget. It takes a single child and positions it
        // in the middle of the parent.
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            GestureDetector(
              onTap: () {
                showNextImage();
              },
              child: Image.asset(_images.length > _imageIndex ? _images[_imageIndex] : 'lib/images/baby_laugh.jpg')
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _showNextImage,
        tooltip: 'Next Laugh',
        child: const Icon(Icons.refresh_sharp),
      ), // This trailing comma makes auto-formatting nicer for build methods.
    );
  }
  
  
}
