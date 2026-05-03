import 'dart:developer';

import 'package:flutter/material.dart';

void main() {
  runApp(App());
}

class App extends StatelessWidget {
  const App({super.key});

  void print(String str) {
    debugPrint(str);
    log(str);
  }

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      home: Scaffold(
        appBar: AppBar(
          title: Center(child: Text("Menu")),
          backgroundColor: Colors.yellow,
        ),
        body: Center(
          child: Text(
            "Hello World",
            style: TextStyle(fontSize: 30, color: Colors.blue),
          ),
        ),

        bottomNavigationBar: BottomAppBar(
          color: Colors.blue,
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceAround,
            children: [
              TextButton(
                onPressed: () {
                  print("Hello");
                },
                child: Text("Item1", style: TextStyle(color: Colors.white)),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
