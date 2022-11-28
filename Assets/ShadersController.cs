using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadersController : MonoBehaviour
{

    //public ComputeShader CellularAutomataShader;
    public ComputeShader RandomShader;

    public ComputeShader SetPreTextureShader;
    public ComputeShader PaintShader;

    public RenderTexture PreResult;
    public RenderTexture Result;


    public bool isPaint;
    public bool isAlive;
    public float radius = 5;
    float posX, posY;

    int width;
    int height;
    private void Start()
    {
        height = Camera.main.pixelHeight;
        width = Camera.main.pixelWidth;
        SetStartProperties();
    }
    private void Update()
    {
        posX = Input.mousePosition.x;
        posY = Input.mousePosition.y;
        if (Input.GetMouseButton(0))
        {
            isPaint = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isPaint = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            isAlive = !isAlive;
        }

    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (Result == null)
        {
            SetStartProperties();
        }
        //CellularAutomataShader.Dispatch(0, Result.width / 8, Result.height / 8, 1);
        RandomShader.Dispatch(0, Result.width / 8, Result.height / 8, 1);

        PaintShader.SetBool("isPaint", isPaint);
        PaintShader.SetBool("isAlive", isAlive);
        PaintShader.SetFloat("BrushPosX", Input.mousePosition.x);
        PaintShader.SetFloat("BrushPosY", Input.mousePosition.y);
        PaintShader.SetFloat("Radius", radius);

        PaintShader.Dispatch(0, Result.width / 8, Result.height / 8, 1);

        SetPreTextureShader.Dispatch(0, PreResult.width / 8, PreResult.height / 8, 1);

        Graphics.Blit(Result, destination);
    }

    void SetStartProperties()
    {
        Result = new RenderTexture(width, height, 24);
        Result.enableRandomWrite = true;
        Result.Create();

        PreResult = new RenderTexture(width, height, 24);
        PreResult.enableRandomWrite = true;
        PreResult.Create();

        //CellularAutomataShader.SetInt("width", width);
        //CellularAutomataShader.SetInt("height", height);
        //CellularAutomataShader.SetTexture(0, "PreResult", PreResult);
        //CellularAutomataShader.SetTexture(0, "Result", Result);

        RandomShader.SetInt("width", width);
        RandomShader.SetInt("height", height);
        RandomShader.SetTexture(0, "PreResult", PreResult);
        RandomShader.SetTexture(0, "Result", Result);

        SetPreTextureShader.SetTexture(0, "PreResult", PreResult);
        SetPreTextureShader.SetTexture(0, "Result", Result);

        PaintShader.SetTexture(0, "Result", Result);
    }
}
