// Copyright 2014 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


    using System.Collections;
    using UnityEngine;
    [RequireComponent(typeof(Collider))]
    public class SelectorController : MonoBehaviour {
    public Material inactiveMaterial, gazedAtMaterial, turdedOffMaterial;
    private Vector3 startingPosition;
    private Renderer renderer;
    public bool on;
    public bool side;
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        on = false;
        renderer.material = turdedOffMaterial;
    }
    void Start() {
        SetGazedAt(false);
        StartCoroutine("Wait");
    }

    public void SetGazedAt(bool gazedAt) {
            if (gazedAt)
            {
                StartCoroutine("Stared");
            }
            else
            {
                StopCoroutine("Stared");
            }
            if (inactiveMaterial != null && gazedAtMaterial != null && on)
            {
                renderer.material = gazedAt ? gazedAtMaterial : inactiveMaterial;
                return;
            }
    }

    public void Recenter() {
#if !UNITY_EDITOR
      GvrCardboardHelpers.Recenter();
#else
      if (GvrEditorEmulator.Instance != null) {
        GvrEditorEmulator.Instance.Recenter();
      }
#endif  // !UNITY_EDITOR
    }

    public void selected()
    {
        if (side)
        {
            this.GetComponentInParent<ScreenController>().answerRight();
        }else
        {
            this.GetComponentInParent<ScreenController>().answerLeft();
        }
       
    }
    IEnumerator Stared()
    {
            while (true && on)
            {
                yield return new WaitForSeconds(3);
                selected();
            }           
    }

    IEnumerator Wait()
    {
        renderer.material = turdedOffMaterial;
        yield return new WaitForSeconds(3);
        this.on = true;
        renderer.material = inactiveMaterial;
    }
  }
