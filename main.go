package main

import (
	"fmt"
	"os"

	"github.com/avestalang/core/compiler"
	"github.com/avestalang/core/lib"
)

func main() {
	inputAddress := os.Args[1]

	content, err := lib.Reader(inputAddress)

	if err != nil {
		content = os.Args[1]
	}

	result, _ := compiler.Compile(content)

	fmt.Println(result)
}
